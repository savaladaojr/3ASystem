# 🗺️ Mapa Mental - Feature Operations Management

## Estrutura Hierárquica Completa

```
╔══════════════════════════════════════════════════════════════════════════════╗
║                    OPERATIONS MANAGEMENT FEATURE                             ║
║                     (Inclusão de Operations nas Functionalities)             ║
╚══════════════════════════════════════════════════════════════════════════════╝

┌──────────────────────────────────────────────────────────────────────────────┐
│                           PLANEJAMENTO ESTRATÉGICO                           │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ VISÃO
│  ├─ Gerenciar operações de funcionalidades centralizadamente
│  ├─ Interface intuitiva (modal)
│  └─ CRUD + operações padrão
│
├─ OBJETIVO
│  ├─ Permitir admin gerenciar ações de cada funcionalidade
│  ├─ Validar unicidade de código/friendlyId
│  └─ Oferecer templates padrão (View, Create, Update, Delete)
│
├─ ESCOPO
│  ├─ ✅ CRUD de operações
│  ├─ ✅ Ativar/Desativar
│  ├─ ✅ Operações padrão
│  ├─ ✅ Validações de negócio
│  ├─ ✅ Feedback visual
│  ├─ ❌ Permissionamento granular
│  └─ ❌ API REST (apenas UI)
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                           ARQUITETURA TÉCNICA                               │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ DOMAIN LAYER
│  ├─ Entity: Operation
│  │  ├─ Propriedades: Id, FunctionalityId, Name, Code, IsActive, FriendlyId
│  │  ├─ Métodos: Create, Update, Enable, Disable
│  │  └─ Validações: unicidade de Code/FriendlyId
│  │
│  └─ Errors: OperationErrors
│     ├─ NotFound(id)
│     ├─ CodeNotUnique
│     ├─ FriendlyIdNotUnique
│     └─ InvalidFunctionalityId
│
├─ APPLICATION LAYER
│  │
│  ├─ COMMANDS
│  │  ├─ CreateOperationCommand
│  │  │  ├─ Handler: Validar unicidade, criar, persistir
│  │  │  ├─ Validator: Name, Code, FriendlyId obrigatórios
│  │  │  └─ Response: OperationResponse
│  │  │
│  │  ├─ UpdateOperationCommand
│  │  │  ├─ Handler: Validar unicidade (exceto atual), atualizar
│  │  │  └─ Validator: Mesmo do Create
│  │  │
│  │  ├─ DeleteOperationCommand
│  │  │  ├─ Handler: Validar dependências, deletar
│  │  │  └─ Validator: OperationId obrigatório
│  │  │
│  │  └─ EnableDisableOperationCommand
│  │     ├─ Handler: Alternar IsActive
│  │     └─ Validator: OperationId obrigatório
│  │
│  ├─ QUERIES
│  │  ├─ GetOperationsByFunctionalityQuery
│  │  │  ├─ Handler: Retorna List<OperationResponse>
│  │  │  └─ Ordernar por CreatedAt
│  │  │
│  │  └─ GetOperationByIdQuery (opcional)
│  │     └─ Handler: Retorna OperationResponse ou null
│  │
│  └─ RESPONSES
│     ├─ OperationResponse (DTO)
│     │  ├─ Id, FunctionalityId, Name, Code, IsActive
│     │  ├─ FriendlyId, CreatedAt, LastUpdatedAt
│     │  └─ Extensions: ToOperationResponse()
│     │
│     └─ Standard Operations Batch Response (futuro)
│
├─ INFRASTRUCTURE LAYER
│  ├─ Repositories
│  │  ├─ OperationRepository: IOperationRepository
│  │  │  ├─ GetByCodeAsync(code): Operation?
│  │  │  └─ GetByFriendlyIdAsync(friendlyId): Operation?
│  │  │
│  │  └─ IOperationRepository: IRepository<Operation, OperationId>
│  │
│  └─ Configuration
│     └─ OperationConfiguration (já existe)
│        ├─ Mapeamento: Id, FunctionalityId
│        ├─ Índices: Code, FriendlyId (únicos)
│        ├─ MaxLengths: Name(100), Code(50), FriendlyId(50)
│        └─ Relacionamento: Functionality one-to-many
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                        APRESENTAÇÃO / UI LAYER                              │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ PÁGINAS (Modificadas)
│  └─ FunctionalitiesList.razor
│     ├─ Adicionar coluna: "Gerenciar Operações" na tabela
│     ├─ Ícone: Tune (⚙️)
│     ├─ Cor: Primary (azul)
│     └─ Ação: Abre OperationsModal passando FunctionalityId
│
├─ COMPONENTES (Novos)
│  │
│  ├─ OperationsModal.razor + .cs + .css
│  │  ├─ Estrutura
│  │  │  ├─ Header: "Gerenciar Operações: [Functionality Name]"
│  │  │  ├─ Section 1: Operações Padrão
│  │  │  │  └─ Botão: "Adicionar Operações Padrão" (Success/verde)
│  │  │  ├─ Section 2: Operações Cadastradas
│  │  │  │  └─ Tabela com Name, Code, FriendlyId, IsActive, Actions
│  │  │  ├─ Section 3: Formulário (Show/Hide)
│  │  │  │  └─ OperationForm component
│  │  │  └─ Footer: [Cancelar] [Fechar]
│  │  │
│  │  ├─ Comportamento
│  │  │  ├─ OnInit: LoadOperations via GetOperationsByFunctionalityQuery
│  │  │  ├─ Tabela Actions:
│  │  │  │  ├─ Toggle ✓/✗ (Enable/Disable)
│  │  │  │  ├─ Edit ✏️ (abre formulário)
│  │  │  │  └─ Delete 🗑 (pede confirmação)
│  │  │  ├─ Padrão Button: Valida + Cria 4 ops + Desabilita
│  │  │  └─ Feedback: Snackbars para sucesso/erro/aviso
│  │  │
│  │  └─ State Management
│  │     ├─ _operations: List<OperationResponse>
│  │     ├─ _isLoading: bool
│  │     ├─ _isProcessing: bool
│  │     ├─ _showForm: bool
│  │     ├─ _editingOperation: OperationResponse?
│  │     ├─ _formMode: "Create" | "Edit"
│  │     └─ _error: string
│  │
│  └─ OperationForm.razor + .cs + .css
│     ├─ Modo Create
│     │  ├─ Campos vazios
│     │  └─ Ao salvar: CreateOperationCommand
│     │
│     ├─ Modo Edit
│     │  ├─ Campos preenchidos com dados
│     │  └─ Ao salvar: UpdateOperationCommand
│     │
│     ├─ Validações em Tempo Real
│     │  ├─ Name: Obrigatório, máx 100
│     │  ├─ Code: Obrigatório, máx 50, único
│     │  └─ FriendlyId: Obrigatório, máx 50, único
│     │
│     └─ Callbacks
│        ├─ OnSave: Recarrega modal
│        └─ OnCancel: Fecha formulário
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                           FLUXOS DE NEGÓCIO                                 │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ FLUXO 1: Abrir Modal
│  └─ User → Clica ⚙️ → Modal abre → Carrega operações → Exibe tabela
│
├─ FLUXO 2: Criar Operação Customizada
│  └─ Usuário → Preenche formulário → Valida → Cria → Aparece na tabela
│
├─ FLUXO 3: Editar Operação
│  └─ Usuário → Clica ✏️ → Formulário carrega → Modifica → Atualiza
│
├─ FLUXO 4: Deletar Operação
│  └─ Usuário → Clica 🗑 → Confirmação → Deleta → Desaparece
│
├─ FLUXO 5: Ativar/Desativar Operação
│  └─ Usuário → Clica toggle → Estado muda → Atualiza no banco
│
├─ FLUXO 6: Adicionar Operações Padrão ⭐ CRÍTICO
│  │
│  ├─ Validação
│  │  ├─ Verifica se View/Create/Update/Delete já existem
│  │  └─ Mantém as existentes; conflito de chave interrompe antes da inserção
│  │
│  ├─ Criação (SaveChanges atômico)
│  │  └─ Cria apenas as operações padrão faltantes
│  │
│  ├─ Sucesso
│  │  ├─ Recarrega tabela
│  │  └─ Snackbar de sucesso
│  │
│  └─ Erro
│     ├─ Não insere parcialmente em caso de colisão
│     └─ Snackbar com mensagem de erro
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                          VALIDAÇÕES E REGRAS                                │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ CRIAR OPERAÇÃO
│  ├─ FunctionalityId deve existir ✓
│  ├─ Name obrigatório, máx 100 ✓
│  ├─ Code obrigatório, máx 50, único GLOBALMENTE ✓
│  └─ FriendlyId obrigatório, máx 50, único GLOBALMENTE ✓
│
├─ EDITAR OPERAÇÃO
│  ├─ OperationId deve existir ✓
│  ├─ Name obrigatório, máx 100 ✓
│  ├─ Code único (excluindo registro atual) ✓
│  └─ FriendlyId único (excluindo registro atual) ✓
│
├─ DELETAR OPERAÇÃO
│  ├─ OperationId deve existir ✓
│  └─ (Futuro) Verificar dependências (RoleOperations)
│
├─ OPERAÇÕES PADRÃO
│  ├─ Validar cada um: View, Create, Update, Delete
│  ├─ Não adicionar se já existe (duplicata check)
│  ├─ Transação atômica (tudo ou nada)
│  └─ Feedback específico por operação
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                          FEEDBACK & UX                                      │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ SNACKBARS (Success - Verde)
│  ├─ "Operation created successfully!"
│  ├─ "Operation updated successfully!"
│  ├─ "Operation deleted successfully!"
│  ├─ "4 standard operation(s) added successfully!"
│  ├─ "Operation activated!"
│  └─ "Operation deactivated!"
│
├─ SNACKBARS (Warning - Laranja)
│  ├─ "Some standard operations already exist."
│  └─ "All standard operations already exist."
│
├─ SNACKBARS (Error - Vermelho)
│  ├─ "Operation code must be unique."
│  ├─ "Operation friendly ID must be unique."
│  ├─ "The specified functionality does not exist."
│  └─ "Error: [descrição específica]"
│
├─ DIALOGS (Confirmação)
│  ├─ Deletar: "Are you sure you want to delete '[Name]'?"
│  ├─ Botões: [Cancel] [Delete]
│  └─ Cor: Error (vermelho)
│
├─ ALERTS (Inline)
│  ├─ Info: "No operations found for this functionality."
│  └─ Error: Exibir validações do formulário
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                           CORES E ESTILOS                                   │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ MudBlazor Colors
│  ├─ Primary (Azul):     #1976d2 - Ações gerais
│  ├─ Success (Verde):    #4caf50 - Criar/Adicionar
│  ├─ Info (Cyan):        #00bcd4 - Editar/Informativo
│  ├─ Error (Vermelho):   #f44336 - Deletar/Erro
│  ├─ Warning (Laranja):  #ff9800 - Ativo/Aviso
│  ├─ Tertiary (Cyan):    #00bcd4 - Inativo
│  └─ Secondary (Cinza):  #757575 - Cancelar
│
├─ Typography
│  ├─ Modal Title:    Typo.h6 (font-weight: 600)
│  ├─ Section Title:  Typo.subtitle2 (bold)
│  ├─ Table Headers:  font-weight: bold
│  ├─ Form Labels:    Typo.caption
│  └─ Errors:         Typo.caption, Color.Error
│
├─ Spacing
│  ├─ Modal Padding:    8px
│  ├─ Section Margin:   24px (bottom)
│  ├─ Field Margin:     16px (bottom)
│  ├─ Button Gap:       12px
│  └─ Border Radius:    4px
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                         IMPLEMENTAÇÃO PHASES                                │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ PHASE 1: Domain Layer (1-2h)
│  ├─ Criar OperationErrors.cs
│  └─ Atualizar Operation.cs (adicionar Update se necessário)
│
├─ PHASE 2: Application Layer (3-4h)
│  ├─ 4 Commands + Handlers + Validators
│  ├─ 2 Queries + Handlers
│  ├─ OperationResponse + Extensions
│  └─ Expandir OperationRepository
│
├─ PHASE 3: Infrastructure (0.5h)
│  └─ Validar MediatR registration (automático)
│
├─ PHASE 4: Web UI (4-5h)
│  ├─ OperationsModal.razor + .cs + .css
│  ├─ OperationForm.razor + .cs + .css
│  └─ Modificar FunctionalitiesList
│
├─ PHASE 5: Testing (2-3h)
│  ├─ Unit Tests (Domain)
│  ├─ Integration Tests (Application)
│  └─ Manual Testing (UI)
│
└─ PHASE 6: Refinement (1-2h)
   ├─ UX Polish
   ├─ CSS Tweaks
   └─ Message Refinement
│
└─ TOTAL: ~16.5h
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                        ARQUIVOS A CRIAR/MODIFICAR                           │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ CRIAR (13 novos)
│  ├─ Domain: OperationErrors.cs
│  ├─ Commands (4):
│  │  ├─ CreateOperation (3 files: Command, Handler, Validator)
│  │  ├─ UpdateOperation (3 files)
│  │  ├─ DeleteOperation (3 files)
│  │  └─ EnableDisableOperation (3 files)
│  ├─ Queries (2):
│  │  ├─ GetOperationsByFunctionality (2 files)
│  │  └─ GetOperationById (2 files - opcional)
│  ├─ Responses: OperationResponse.cs, OperationResponseExtensions.cs
│  └─ UI (6):
│     ├─ OperationsModal.razor + .cs + .css (3)
│     └─ OperationForm.razor + .cs + .css (3)
│
├─ MODIFICAR (2)
│  ├─ Operation.cs (adicionar Update)
│  ├─ OperationRepository.cs (adicionar GetByCode, GetByFriendlyId)
│  ├─ IOperationRepository.cs (adicionar métodos)
│  ├─ FunctionalitiesList.razor (adicionar botão)
│  ├─ FunctionalitiesList.razor.cs (adicionar método)
│  └─ Program.cs (se necessário registrar MediatR)
│
└──────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────────────┐
│                         DOCUMENTAÇÃO CRIADA                                 │
├──────────────────────────────────────────────────────────────────────────────┤
│
├─ 📋 README.md
│  └─ Índice e guia de navegação
│
├─ ⭐ OPERATIONS_SUMMARY.md
│  └─ Visão executiva (10 min read)
│
├─ 📋 OPERATIONS_FEATURE_PLAN.md
│  └─ Documento principal completo (30-40 min read)
│
├─ 💻 OPERATIONS_TECHNICAL_GUIDE.md
│  └─ Step-by-step com código (referência contínua)
│
├─ 🎨 OPERATIONS_VISUAL_DESIGN.md
│  └─ Especificação visual/UX (25 min read)
│
├─ ⚡ QUICK_START.md
│  └─ Resumo rápido para implementação (10 min read)
│
└─ 🗺️ MAPA_MENTAL.md
   └─ Estrutura hierárquica visual (este arquivo)
│
└──────────────────────────────────────────────────────────────────────────────┘

```

---

## 📌 Referência Rápida

### Que documento ler?

| Seu Papel | Comece com | Depois leia |
|-----------|-----------|-----------|
| **Gerente/PO** | OPERATIONS_SUMMARY.md | OPERATIONS_FEATURE_PLAN.md |
| **Arquiteto/Lead** | OPERATIONS_FEATURE_PLAN.md | OPERATIONS_TECHNICAL_GUIDE.md |
| **Developer** | QUICK_START.md | OPERATIONS_TECHNICAL_GUIDE.md |
| **QA/Tester** | OPERATIONS_SUMMARY.md | OPERATIONS_VISUAL_DESIGN.md |
| **Designer** | OPERATIONS_VISUAL_DESIGN.md | OPERATIONS_SUMMARY.md |

---

## 🎯 Próximos Passos

1. ✅ **Implementação** (código e testes automatizados)
2. ⏳ **Migração** (`IncreaseOperationFriendlyIdLength`)
3. ⏳ **Validação manual** dos fluxos da UI
4. ⏳ **Revisão, merge e deploy**

---

**Estado e validações:** consulte [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md).
