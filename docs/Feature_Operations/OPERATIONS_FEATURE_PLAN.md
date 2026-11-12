# Plano de Desenvolvimento - Inclusão de Operations nas Functionalities

**Data:** Março 2025  
**Status:** Planejamento  
**Versão:** 1.0

---

## 1. Visão Geral

Implementar um sistema completo para gerenciar **Operations** (Operações) associadas às **Functionalities** (Funcionalidades) do sistema. A solução incluirá uma interface modal para CRUD de operações, listagem de operações cadastradas e funcionalidade para adicionar operações padrão (View, Create, Update, Delete).

---

## 2. Contexto Atual

### Entidades Existentes

- **Functionality**: Entidade de domínio representando uma funcionalidade de um módulo
  - Relacionamento: `ICollection<Operation>? Operations` (um-para-muitos)
  - Propriedades: Id, ModuleId, Name, Abbreviation, Route, IconUrl, Description, FriendlyId, IsActive, IsPartOfMenu

- **Operation**: Entidade de domínio representando uma operação/ação em uma funcionalidade
  - Propriedades: Id, FunctionalityId, Name, Code, IsActive, FriendlyId
  - Métodos de domínio: Create, Update, Enable, Disable
  - Banco de dados: Relacionamento one-to-many com Functionality configurado

### Infraestrutura

- **Repository Pattern**: IOperationRepository e OperationRepository implementados
- **MediatR**: Command/Query handlers para orquestração de use cases
- **Entity Framework Core**: ORM para persistência
- **MudBlazor**: Framework UI com componentes Material Design

### Padrões de Arquitetura Identificados

1. **Clean Architecture** com camadas:
   - Domain (entidades e eventos de domínio)
   - Application (use cases, handlers, responses)
   - Infrastructure (repositórios, configurações EF)
   - WebUI (componentes Razor, páginas)

2. **CQRS Pattern**: Separação entre Commands e Queries

3. **Validação**: Utiliza FluentValidation com validators específicos por command

4. **Respostas Padronizadas**: DTOs com sufixo `Response` e `DetailedResponse`

5. **UI Components**: Uso de MudBlazor para componentes Material Design

### Esquema de Cores Identificado

- **Primary Color**: Azul (#1976d2 - padrão MudBlazor)
- **Secondary Color**: Roxo/Cinza
- **Tertiary Color**: Verde/Cyan
- **Warning Color**: Laranja
- **Error Color**: Vermelho
- **Info Color**: Cyan
- **Success Color**: Verde (#4caf50)
- **Sidebar**: Gradiente azul-escuro a roxo-escuro (rgb(5, 39, 103) → #3a0647)

---

## 3. Requisitos Funcionais

### 3.1 Botão de Ação na Listagem de Functionalities

- Adicionar botão "Gerenciar Operações" em cada linha da tabela de funcionalidades
- Posicionar junto aos botões de Edit e Delete
- Ícone sugerido: `Icons.Material.Filled.Tune` ou `Icons.Material.Filled.Assignment`
- Cor: Primary
- Ao clicar, abrir modal de gerenciamento de operações

### 3.2 Modal de Gerenciamento de Operações

O modal exibirá:

#### Header
- Título: "Gerenciar Operações - [Nome da Funcionalidade]"
- Ícone: Assignment ou Tune

#### Conteúdo Principal - Seção de Operações Cadastradas

- **Tabela com operações existentes** contendo:
  - Coluna: Name
  - Coluna: Code
  - Coluna: FriendlyId
  - Coluna: IsActive (toggle/checkbox ou badge de status)
  - Coluna: Ações
    - **Ativar/Desativar**: Toggle com ícone `Icons.Material.Filled.CheckCircle`
      - Desativada: Cor Tertiary
      - Ativada: Cor Warning
    - **Editar**: Botão com ícone `Icons.Material.Filled.Edit` (Color.Info)
    - **Deletar**: Botão com ícone `Icons.Material.Filled.Delete` (Color.Error)

- **Comportamento de operações**:
  - Ativar/Desativar: Chamada direta do handler sem modal
  - Editar: Abre sub-modal inline ou formulário expansível
  - Deletar: Solicita confirmação com dialog antes de remover

#### Seção de Operações Padrão

- **Botão**: "Adicionar Operações Padrão"
- Comportamento:
  - Garante que existam 4 operações: View, Create, Update e Delete.
  - Os códigos usam `{Abbreviation}.{Name}` e os FriendlyIds usam `{FriendlyId}-{Name}`.
  - Operações existentes com os mesmos Code e FriendlyId na funcionalidade são mantidas; as faltantes são criadas.
  - Uma colisão com outro registro retorna conflito sem inserir parcialmente as operações.
  - Após adição, atualiza a tabela com as novas operações
  - Exibe feedback visual (Snackbar)

#### Formulário de Criação/Edição (Inline ou Sub-Modal)

- **Campo: Name** (obrigatório, até 100 caracteres)
- **Campo: Code** (obrigatório, até 50 caracteres, deve ser único)
- **Campo: FriendlyId** (obrigatório, até 50 caracteres, deve ser único)
- **Botões**:
  - Cancelar: Fechar o formulário
  - Salvar: Persistir alterações

#### Footer do Modal

- **Botão: Cancelar** (Variant.Outlined, Color.Secondary)
- **Botão: Fechar** (Variant.Filled, Color.Primary)
- Observação: Não há "Salvar" no footer pois cada operação é salva individualmente

### 3.3 Confirmação de Exclusão

- Dialog de confirmação padrão com mensagem: "Tem certeza que deseja deletar a operação '[Name]'?"
- Botões: Cancelar e Confirmar (Color.Error)

---

## 4. Requisitos Não-Funcionais

### 4.1 Performance
- Carregamento lazy das operações ao abrir o modal
- Spinner/loader visual durante requisições
- Paginação não necessária (operações típicas: 4-10 por funcionalidade)

### 4.2 UX/UI
- Responsividade em diferentes tamanhos de tela
- Confirmações visuais (Snackbar) para ações críticas
- Estados de Loading/Error tratados
- Validação de formulário em tempo real
- Desabilitar botões durante processamento

### 4.3 Segurança
- Validação de autorização (funcionalidade pertence ao usuário)
- Sanitização de inputs
- CSRF protection via MudBlazor

---

## 5. Arquitetura de Implementação

### 5.1 Estrutura de Pastas

```
src/
├── 3ASystem.Domain/
│   └── Entities/Operations/
│       ├── Operation.cs (existente)
│       ├── OperationId.cs (existente)
│       └── OperationErrors.cs (nova)
│
├── 3ASystem.Application/
│   └── UseCases/Operations/
│       ├── Commands/
│       │   ├── CreateOperation/
│       │   │   ├── CreateOperationCommand.cs (nova)
│       │   │   ├── CreateOperationCommandHandler.cs (nova)
│       │   │   └── CreateOperationCommandValidator.cs (nova)
│       │   ├── UpdateOperation/
│       │   │   ├── UpdateOperationCommand.cs (nova)
│       │   │   ├── UpdateOperationCommandHandler.cs (nova)
│       │   │   └── UpdateOperationCommandValidator.cs (nova)
│       │   ├── DeleteOperation/
│       │   │   ├── DeleteOperationCommand.cs (nova)
│       │   │   ├── DeleteOperationCommandHandler.cs (nova)
│       │   │   └── DeleteOperationCommandValidator.cs (nova)
│       │   └── EnableDisableOperation/
│       │       ├── EnableDisableOperationCommand.cs (nova)
│       │       ├── EnableDisableOperationCommandHandler.cs (nova)
│       │       └── EnableDisableOperationCommandValidator.cs (nova)
│       ├── Queries/
│       │   ├── GetOperationsByFunctionality/
│       │   │   ├── GetOperationsByFunctionalityQuery.cs (nova)
│       │   │   └── GetOperationsByFunctionalityHandler.cs (nova)
│       │   └── GetOperationById/
│       │       ├── GetOperationByIdQuery.cs (nova)
│       │       └── GetOperationByIdHandler.cs (nova)
│       └── Responses/
│           ├── OperationResponse.cs (nova)
│           └── OperationResponseExtensions.cs (nova)
│
├── 3ASystem.Infrastructure/
│   └── Data/
│       └── Repositories/
│           └── OperationRepository.cs (existente - expandir se necessário)
│
└── 3ASystem.WebUI.Server/
    └── Components/
        └── Pages/
            └── Functionalities/
                ├── FunctionalitiesList.razor (modificar)
                ├── FunctionalitiesList.razor.cs (modificar)
                ├── OperationsModal.razor (nova)
                ├── OperationsModal.razor.cs (nova)
                ├── OperationsModal.razor.css (nova)
                ├── OperationForm.razor (nova)
                ├── OperationForm.razor.cs (nova)
                └── OperationForm.razor.css (nova)
```

### 5.2 Entidades de Domínio

#### OperationErrors (Nova)

```csharp
namespace _3ASystem.Domain.Entities.Operations;

public class OperationErrors
{
    public static Error NotFound(OperationId id) => 
        Error.NotFound("Operation.NotFound", $"Operation with ID {id} was not found.");

    public static Error CodeNotUnique => 
        Error.Conflict("Operation.CodeNotUnique", "Operation code must be unique.");

    public static Error FriendlyIdNotUnique => 
        Error.Conflict("Operation.FriendlyIdNotUnique", "Operation friendly ID must be unique.");
}
```

### 5.3 Application Use Cases

#### Responses

**OperationResponse.cs**: DTO para listar operações
- Id
- FunctionalityId
- Name
- Code
- IsActive
- FriendlyId

#### Commands

**CreateOperationCommand.cs**
- FunctionalityId
- Name
- Code
- FriendlyId

**UpdateOperationCommand.cs**
- OperationId
- Name
- Code
- FriendlyId

**DeleteOperationCommand.cs**
- OperationId

**EnableDisableOperationCommand.cs**
- OperationId
- IsActive

#### Queries

**GetOperationsByFunctionalityQuery.cs**
- FunctionalityId
- Retorna: List<OperationResponse>

**GetOperationByIdQuery.cs**
- OperationId
- Retorna: OperationResponse

---

## 6. Componentes Razor (WebUI)

### 6.1 OperationsModal.razor

Componente principal que encapsula todo o modal de gerenciamento.

**Props:**
- `FunctionalityId` (Guid)
- `FunctionalityName` (string)

**Comportamento:**
- Carrega lista de operações ao abrir
- Gerencia estado do formulário
- Coordena callbacks dos sub-componentes

### 6.2 OperationsModal.razor.cs

**Responsabilidades:**
- Injetar IMediator, ISnackbar, IDialogService
- Gerenciar estado da lista de operações
- Executar queries/commands via MediatR
- Tratamento de erros
- Loading states
- Métodos: OnOpen, LoadOperations, AddStandardOperations, etc.

### 6.3 OperationForm.razor

Componente reutilizável para criar/editar operações.

**Modos:**
- Create: Novo formulário vazio
- Edit: Pré-preenchido com dados da operação

**Validações:**
- Name (obrigatório, máx 100)
- Code (obrigatório, máx 50, único)
- FriendlyId (obrigatório, máx 50, único)

### 6.4 Modificações em FunctionalitiesList.razor

- Adicionar coluna de ações: botão "Gerenciar Operações"
- Ao clicar, abrir OperationsModal passando FunctionalityId
- Manter padrão visual de botões pequenos com ícones

---

## 7. Fluxos de Interação

### 7.1 Fluxo - Abrir Modal de Operações

```
Usuário clica "Gerenciar Operações" (botão na tabela)
  ↓
FunctionalitiesList passa FunctionalityId → OperationsModal
  ↓
OperationsModal carrega operações (Query: GetOperationsByFunctionality)
  ↓
Tabela exibe operações com dados carregados
```

### 7.2 Fluxo - Adicionar Operação Padrão

```
Usuário clica "Adicionar Operações Padrão"
  ↓
Verifica cada operação padrão por Code e FriendlyId
  ↓
Se uma chave estiver em conflito com outro registro → retorna conflito sem inserir nada
  ↓
Cria somente as operações que ainda não existem para a funcionalidade
  ↓
Sucesso → Recarregar lista + Snackbar
```

### 7.3 Fluxo - Criar Operação Customizada

```
Usuário clica botão "Adicionar Operação" ou "Nova Operação"
  ↓
OperationForm abre (modo Create)
  ↓
Preenchimento de campos: Name, Code, FriendlyId
  ↓
Validação em tempo real (Code/FriendlyId únicos)
  ↓
Usuário clica "Salvar"
  ↓
Executar Command: CreateOperation
  ↓
Se sucesso → Recarregar lista + Snackbar
  ↓
Se erro → Exibir mensagem de erro no formulário
```

### 7.4 Fluxo - Editar Operação

```
Usuário clica ícone "Edit" em uma operação
  ↓
OperationForm abre (modo Edit) com dados pré-preenchidos
  ↓
Usuário modifica campos
  ↓
Validação em tempo real
  ↓
Usuário clica "Salvar"
  ↓
Executar Command: UpdateOperation
  ↓
Sucesso → Recarregar lista + Snackbar
```

### 7.5 Fluxo - Ativar/Desativar Operação

```
Usuário clica toggle de IsActive
  ↓
Executar Command: EnableDisableOperation
  ↓
Se sucesso → Atualizar estado visual do toggle + Snackbar
  ↓
Se erro → Reverter toggle + Exibir erro
```

### 7.6 Fluxo - Deletar Operação

```
Usuário clica ícone "Delete"
  ↓
Dialog de confirmação: "Tem certeza que deseja deletar '[Name]'?"
  ↓
Se Cancelar → Fechar dialog
  ↓
Se Confirmar → Executar Command: DeleteOperation
  ↓
Sucesso → Recarregar lista + Snackbar
  ↓
Erro → Exibir mensagem de erro em dialog
```

---

## 8. Validações de Negócio

### 8.1 Criação de Operação
- ✓ FunctionalityId deve existir
- ✓ Code deve ser único (no escopo da aplicação)
- ✓ FriendlyId deve ser único (no escopo da aplicação)
- ✓ Name obrigatório e máx 100 caracteres
- ✓ Code obrigatório e máx 50 caracteres
- ✓ FriendlyId obrigatório e máx 50 caracteres

### 8.2 Edição de Operação
- ✓ Mesmas validações de criação
- ✓ Code/FriendlyId único, excluindo o registro atual

### 8.3 Operações Padrão
- ✓ Garantir View/Create/Update/Delete sem duplicar registros correspondentes
- ✓ Colisões impedem inserções parciais; `SaveChanges` persiste o conjunto de inserções atomicamente

### 8.4 Deleção
- ✓ Operação deve existir
- ✓ Impedir a exclusão de uma funcionalidade enquanto existirem operações associadas
- ○ Verificar referências de permissões/roles ao excluir uma operação (fora do modelo atual)

---

## 9. Mensagens de Feedback

### Snackbars (Sucesso - Verde)
- "Operação criada com sucesso!"
- "Operação atualizada com sucesso!"
- "Operação deletada com sucesso!"
- "Operações padrão adicionadas com sucesso!"
- "Operação ativada!"
- "Operação desativada!"

### Dialogs (Erro - Vermelho)
- "Erro ao criar operação: [mensagem específica]"
- "Erro ao atualizar operação: [mensagem específica]"
- "Erro ao deletar operação: [mensagem específica]"
- "Código de operação deve ser único"
- "FriendlyId deve ser único"

### Avisos (Laranja)
- "Algumas operações padrão já existem"
- "Nenhuma operação padrão foi adicionada (todas já existem)"

---

## 10. Estilo e Temas

### Paleta de Cores Aplicada

| Componente | Cor | Uso |
|---|---|---|
| Botão "Gerenciar Operações" | Primary | Ação principal |
| Botão "Adicionar Operações Padrão" | Success | Ação criativa |
| Botão "Editar" | Info | Edição |
| Botão "Deletar" | Error | Ação destrutiva |
| Toggle Ativo | Warning | Status positivo |
| Toggle Inativo | Tertiary | Status negativo |
| Botão "Cancelar" | Secondary | Ação secundária |
| Modal Header | - | Fundo transparente, texto bold |

### CSS Classes Customizadas

**OperationsModal.razor.css**
```css
.operations-modal-container {
    min-width: 600px;
    max-height: 80vh;
    overflow-y: auto;
}

.operations-table {
    margin-top: 16px;
    margin-bottom: 16px;
}

.operations-section {
    margin-bottom: 24px;
}

.section-title {
    font-weight: 600;
    font-size: 1rem;
    margin-bottom: 12px;
    color: rgba(0, 0, 0, 0.87);
}

.standard-operations-button {
    margin-bottom: 16px;
}

.operation-form-container {
    background-color: #fafafa;
    padding: 16px;
    border-radius: 4px;
    border: 1px solid #e0e0e0;
}
```

**OperationForm.razor.css**
```css
.form-field {
    margin-bottom: 16px;
}

.form-actions {
    display: flex;
    justify-content: flex-end;
    gap: 8px;
    margin-top: 20px;
    border-top: 1px solid #e0e0e0;
    padding-top: 16px;
}

.form-error {
    color: #d32f2f;
    font-size: 0.75rem;
    margin-top: 4px;
}
```

---

## 11. Padrão de Implementação - Exemplo

### Exemplo: CreateOperationCommand

```csharp
namespace _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;

public sealed class CreateOperationCommand : ICommand<OperationResponse>
{
    public Guid FunctionalityId { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string FriendlyId { get; set; } = default!;
}

public sealed class CreateOperationCommandValidator : AbstractValidator<CreateOperationCommand>
{
    public CreateOperationCommandValidator()
    {
        RuleFor(x => x.FunctionalityId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FriendlyId).NotEmpty().MaximumLength(50);
    }
}

public sealed class CreateOperationCommandHandler : ICommandHandler<CreateOperationCommand, OperationResponse>
{
    private readonly IOperationRepository _operationRepository;
    private readonly IFunctionalityRepository _functionalityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<OperationResponse>> Handle(
        CreateOperationCommand request,
        CancellationToken cancellationToken)
    {
        // Validações de negócio...
        // Criar entidade...
        // Persistir...
        // Retornar response...
    }
}
```

---

## 12. Testes Sugeridos

### Unit Tests (3ASystem.Tests.Domain)
- ✓ Operation.Create valida campos obrigatórios
- ✓ Operation.Update modifica corretamente
- ✓ Operation.Enable/Disable alterna IsActive

### Integration Tests (3ASystem.Tests.Application)
- ✓ CreateOperationCommandHandler com validações de negócio
- ✓ UpdateOperationCommandHandler com duplicata check
- ✓ DeleteOperationCommandHandler com dependências
- ✓ GetOperationsByFunctionalityHandler retorna lista correta
- ✓ Operações padrão adicionadas sem duplicatas

### UI Tests (Manual)
- ✓ Modal abre/fecha corretamente
- ✓ Tabela carrega operações
- ✓ Formulário valida campos
- ✓ Operações padrão adicionadas e lista atualizada
- ✓ Deleção com confirmação
- ✓ Enable/Disable atualiza estado

---

## 13. Dependências e Configurações

### NuGet Packages (Existentes)
- MudBlazor ✓
- MediatR ✓
- FluentValidation ✓
- EntityFrameworkCore ✓

### Configurações Necessárias

**Program.cs** (WebUI)
- OperationsModal registro em DI (se necessário)
- Handlers MediatR já funcionarão automaticamente via scanning

**Infrastructure**
- OperationRepository já configurado ✓
- DbContext já inclui Operations ✓

---

## 14. Cronograma Estimado

| Fase | Duração | Tarefas |
|---|---|---|
| **1. Setup de Domínio** | 1-2h | Criar OperationErrors, validações |
| **2. Application Layer** | 3-4h | Commands, Queries, Handlers, Validators, Responses |
| **3. UI Components** | 4-5h | OperationsModal, OperationForm, ModalController |
| **4. Integração** | 2-3h | Adicionar botão em FunctionalitiesList, injeções DI |
| **5. Testes** | 2-3h | Unit + Integration + Manual |
| **6. Refinamentos** | 1-2h | UX, validações, mensagens, CSS |
| **Total** | ~13-19h | - |

---

## 15. Checklist de Implementação

### Phase 1: Domain
- [ ] Criar OperationErrors.cs
- [ ] Validar relacionamento Functionality ↔ Operation

### Phase 2: Application
- [ ] CreateOperationCommand + Handler + Validator
- [ ] UpdateOperationCommand + Handler + Validator
- [ ] DeleteOperationCommand + Handler + Validator
- [ ] EnableDisableOperationCommand + Handler + Validator
- [ ] GetOperationsByFunctionalityQuery + Handler
- [ ] GetOperationByIdQuery + Handler
- [ ] OperationResponse + Extensions

### Phase 3: WebUI Components
- [ ] OperationsModal.razor + .cs + .css
- [ ] OperationForm.razor + .cs + .css
- [ ] OperationFormModel / validação frontend
- [ ] Integrar em FunctionalitiesList

### Phase 4: Integração
- [ ] Registrar DI em Program.cs
- [ ] Adicionar botão "Gerenciar Operações" em FunctionalitiesList
- [ ] Testar fluxos end-to-end
- [ ] Validar mensagens de erro
- [ ] Tratamento de edge cases

### Phase 5: Testes
- [ ] Unit tests para commands/queries
- [ ] Integration tests
- [ ] Manual testing (create, edit, delete, enable/disable, padrão)

### Phase 6: Refinements
- [ ] Review de UX/UI
- [ ] Ajustes CSS
- [ ] Validação final de mensagens
- [ ] Documentação de uso (se aplicável)

---

## 16. Referências e Padrões

### Arquivos de Referência no Projeto

- **Padrão Command Handler**: `src\3ASystem.Application\UseCases\Functionalities\Commands\CreateFunctionality\CreateFunctionalityCommandHandler.cs`
- **Padrão Query Handler**: `src\3ASystem.Application\UseCases\Functionalities\Queries\GetFunctionalitiesPaged\GetFunctionalitiesPagedHandler.cs`
- **Padrão Modal Razor**: `src\3ASystem.WebUI.Server\Components\Pages\Functionalities\FunctionalityCreateForm.razor`
- **Padrão Repositório**: `src\3ASystem.Infrastructure\Data\Repositories\FunctionalityRepository.cs`
- **Configuração EF**: `src\3ASystem.Infrastructure\Data\Configurations\OperationConfiguration.cs`

### Convenções de Naming

- Commands: `{Action}{Entity}Command` (ex: CreateOperationCommand)
- Queries: `Get{Entity}{Criteria}Query` (ex: GetOperationsByFunctionalityQuery)
- Handlers: `{CommandOrQuery}Handler`
- Validators: `{CommandOrQuery}Validator`
- Responses: `{Entity}Response` ou `{Entity}DetailedResponse`
- Repositories: `{Entity}Repository`
- Razors: `{Entity}List.razor`, `{Entity}Form.razor`, `{Entity}Modal.razor`

---

## 17. Notas Importantes

1. **Transações**: Ao adicionar operações padrão, usar transaction para garantir all-or-nothing
2. **Validação Única**: Code e FriendlyId são únicos em toda a aplicação (não apenas por funcionalidade)
3. **Soft Delete**: Operações não são realmente deletadas (IsActive = false)? Verificar se segue padrão do projeto
4. **Autorização**: Considerar permissões ao editar/deletar operações de funcionalidades
5. **Auditoria**: Logs de criação/alteração/deleção de operações para rastreabilidade

---

## 18. Aprovações

- [ ] Arquiteto/Lead Dev
- [ ] Product Owner
- [ ] QA Lead

**Documento criado em:** Março 2025  
**Versão:** 1.0 - Planejamento Inicial
