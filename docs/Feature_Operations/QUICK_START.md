# ⚡ Quick Start - Implementação das Operations

## 🎯 Objetivo Final

Permitir que administradores gerenciem operações (ações/permissões) de cada funcionalidade através de um modal intuitivo com CRUD completo.

---

## 📐 Arquitetura Simplificada

```
┌─────────────────────────────────────────────────┐
│              UI Layer (Razor)                    │
│  FunctionalitiesList → [Botão] → OperationsModal
│                                    ├─ Tabela    
│                                    ├─ Form     
│                                    └─ Padrão   
└────────────┬────────────────────────────────────┘
             │
┌────────────▼────────────────────────────────────┐
│         Application Layer (MediatR)              │
│  CreateOperation, UpdateOperation, DeleteOperation
│  GetOperationsByFunctionality, EnableDisableOperation
└────────────┬────────────────────────────────────┘
             │
┌────────────▼────────────────────────────────────┐
│           Domain Layer (Entities)                │
│  Operation + OperationErrors                     │
└────────────┬────────────────────────────────────┘
             │
┌────────────▼────────────────────────────────────┐
│    Infrastructure (Repository + EF)             │
│  OperationRepository                             │
└─────────────────────────────────────────────────┘
```

---

## 📋 Checklist de Implementação Rápido

### ✅ PART 1: Domain Layer (1-2h)

```
[ ] Criar: src\3ASystem.Domain\Entities\Operations\OperationErrors.cs
[ ] Atualizar: src\3ASystem.Domain\Entities\Operations\Operation.cs
    - Adicionar método Update() se não existir
```

### ✅ PART 2: Application Layer (3-4h)

```
[ ] Criar: CreateOperationCommand + Validator + Handler
[ ] Criar: UpdateOperationCommand + Validator + Handler  
[ ] Criar: DeleteOperationCommand + Validator + Handler
[ ] Criar: EnableDisableOperationCommand + Handler
[ ] Criar: GetOperationsByFunctionalityQuery + Handler
[ ] Criar: GetOperationByIdQuery + Handler (opcional)
[ ] Criar: OperationResponse + Extensions
[ ] Atualizar: IOperationRepository + OperationRepository
    - Adicionar GetByCodeAsync()
    - Adicionar GetByFriendlyIdAsync()
```

### ✅ PART 3: Infrastructure (0.5h)

```
[ ] Validar: Program.cs registra MediatR handlers automaticamente
    (ou adicionar se necessário)
```

### ✅ PART 4: Web UI (4-5h)

```
[ ] Criar: src\3ASystem.WebUI.Server\Components\Pages\Functionalities\
           OperationsModal.razor + .razor.cs + .razor.css

[ ] Criar: src\3ASystem.WebUI.Server\Components\Pages\Functionalities\
           OperationForm.razor + .razor.cs + .razor.css

[ ] Modificar: FunctionalitiesList.razor
    - Adicionar botão "Gerenciar Operações" na tabela

[ ] Modificar: FunctionalitiesList.razor.cs
    - Adicionar método OpenOperationsModal()
    - Adicionar injeção de DialogService
```

### ✅ PART 5: Testes (2-3h)

```
[ ] Criar: Tests unit - Operation domain entity
[ ] Criar: Tests integration - CreateOperationCommandHandler
[ ] Criar: Tests integration - UpdateOperationCommandHandler
[ ] Criar: Tests integration - DeleteOperationCommandHandler
[ ] Executar: Manual testing de todos os fluxos
```

---

## 🚀 Implementação Passo-a-Passo

### Passo 1: Estrutura de Diretórios

```powershell
# Criar diretório de Operations (Application)
New-Item -Path "src\3ASystem.Application\UseCases\Operations" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Commands" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Commands\CreateOperation" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Commands\UpdateOperation" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Commands\DeleteOperation" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Commands\EnableDisableOperation" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Queries" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Queries\GetOperationsByFunctionality" -ItemType Directory -Force
New-Item -Path "src\3ASystem.Application\UseCases\Operations\Responses" -ItemType Directory -Force
```

### Passo 2: Domain Layer

1. Copiar código de **OPERATIONS_TECHNICAL_GUIDE.md** → Step 1.1
2. Criar `OperationErrors.cs`
3. Validar `Operation.cs` possui método `Update()`

### Passo 3: Application Layer

1. Copiar código de **OPERATIONS_TECHNICAL_GUIDE.md** → Steps 2.1 a 2.8
2. Implementar cada command/query seguindo os exemplos
3. Expandir OperationRepository com novos métodos
4. Atualizar IOperationRepository interface

### Passo 4: Web UI

1. Copiar código de **OPERATIONS_TECHNICAL_GUIDE.md** → Steps 4.1 a 4.6
2. Criar `OperationsModal.razor` com toda lógica
3. Criar `OperationForm.razor` para formulário
4. Criar arquivos `.css`
5. Modificar `FunctionalitiesList.razor` e `.cs`

### Passo 5: Testes

1. Criar testes unitários (Operation domain)
2. Criar testes de integração (handlers)
3. Testar manualmente cada fluxo

---

## 🎨 Interface Final (Resumo Visual)

### FunctionalitiesList (Modificado)
```
┌─────────────────────────────────────────────┐
│ Functionalities                      [+ New]│
├─────────────────────────────────────────────┤
│ Application │ Module │ Icon │ Name │ Actions
│ App1        │ Mod1   │ 📦   │ Users│ ⚙️ ✏️ 🗑
│ App1        │ Mod1   │ 📦   │ Docs │ ⚙️ ✏️ 🗑
└─────────────────────────────────────────────┘
                     ↓ Clica em ⚙️
                  Modal Abre ↓

┌────────────────────────────────────────────────┐
│ 📋 Gerenciar Operações: Users           [X]   │
├────────────────────────────────────────────────┤
│ [✔ Adicionar Operações Padrão]                │
│                                                │
│ Name    │ Code   │ Friendly │ Active │ Actions
│ View    │ VIEW   │ view     │   ✓    │ ⭕ ✏️ 🗑
│ Create  │ CREATE │ create   │   ✓    │ ⭕ ✏️ 🗑
│ Update  │ UPDATE │ update   │   ✓    │ ⭕ ✏️ 🗑
│ Delete  │ DELETE │ delete   │   ✗    │ ⭕ ✏️ 🗑
│                                                │
│                 [Cancelar] [Fechar]           │
└────────────────────────────────────────────────┘
```

---

## 💡 Dicas de Implementação

### ✓ Copie Padrões Existentes
- `CreateFunctionalityCommandHandler` → base para `CreateOperationCommandHandler`
- `FunctionalitiesList.razor` → base para adicionar botão
- `FunctionalityCreateForm.razor` → base para `OperationForm.razor`

### ✓ Reutilize Componentes
- `ModalComponent.razor` já existe → pode usar como base
- `MudBlazor` componentes → Button, Table, TextField, etc.

### ✓ Valide Incrementalmente
```powershell
# Build incremental
dotnet build src\3ASystem.Domain
dotnet build src\3ASystem.Application
dotnet build src\3ASystem.WebUI.Server
```

### ✓ Teste Manualmente os Fluxos
1. **Criar**: Abre modal → preenche campos → salva → aparece na tabela
2. **Editar**: Clica ✏️ → carrega dados → modifica → salva
3. **Deletar**: Clica 🗑 → confirmação → deleta → desaparece
4. **Ativar/Desativar**: Clica ⭕ → alterna estado
5. **Padrão**: Clica botão padrão → adiciona 4 ops → desabilita botão

---

## 📊 Componentes Principais

### 1️⃣ OperationsModal.razor
```
Responsabilidades:
- Carregar operações via MediatR
- Exibir tabela de operações
- Gerenciar abertura/fechamento de formulário
- Coordenar callbacks dos sub-componentes
- Adicionar operações padrão
- Exibir feedback visual

Props:
- FunctionalityId: Guid
- FunctionalityName: string
```

### 2️⃣ OperationForm.razor
```
Responsabilidades:
- Renderizar formulário (Create ou Edit)
- Validar campos em tempo real
- Enviar command via MediatR
- Retornar callback de sucesso/cancel

Props:
- Mode: "Create" ou "Edit"
- Operation: OperationResponse (para Edit)
- FunctionalityId: Guid
- OnSave: EventCallback
- OnCancel: EventCallback
```

### 3️⃣ CreateOperationCommandHandler
```
Responsabilidades:
- Validar FunctionalityId existe
- Validar Code é único
- Validar FriendlyId é único
- Criar entidade Operation
- Persistir em banco
- Retornar OperationResponse
```

---

## 🔍 Validações Críticas

```csharp
// Code único GLOBALMENTE (não apenas por funcionalidade)
var existingCode = await _operationRepository.GetByCodeAsync(code);
if (existingCode != null) return CodeNotUnique;

// FriendlyId único GLOBALMENTE
var existingFriendlyId = await _operationRepository.GetByFriendlyIdAsync(friendlyId);
if (existingFriendlyId != null) return FriendlyIdNotUnique;

// Functionality existe
var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);
if (functionality == null) return InvalidFunctionalityId;
```

---

## 🎯 Estados a Testar

### Modal
- [ ] Loading (spinner exibe)
- [ ] Loaded (tabela carrega)
- [ ] Formulário aberto (Create)
- [ ] Formulário aberto (Edit)
- [ ] Erro exibindo

### Tabela
- [ ] Operação ativa (toggle warning)
- [ ] Operação inativa (toggle tertiary)
- [ ] Linha adicionada com fade-in
- [ ] Linha removida após delete

### Botão Padrão
- [ ] Habilitado (nenhuma operação padrão existe)
- [ ] Desabilitado (todas existem)
- [ ] Loading (adicionando)

### Formulário
- [ ] Campo vazio (erro)
- [ ] Campo com erro (validação)
- [ ] Campo válido (sem erro)
- [ ] Submitável (todos campos válidos)

---

## 🚦 Fluxos Críticos

### Fluxo 1: Adicionar Operações Padrão
```
Usuário clica "Adicionar Operações Padrão"
  ├─ Verificar Code e FriendlyId de View/Create/Update/Delete
  ├─ Manter operações já existentes para essa funcionalidade
  ├─ Retornar conflito antes de inserir se houver colisão com outro registro
  ├─ Criar somente as operações faltantes (SaveChanges atômico)
  ├─ Recarregar tabela
  └─ Exibir Snackbar de sucesso
```

### Fluxo 2: Deletar com Confirmação
```
Usuário clica ícone Deletar
  ├─ Abrir Dialog "Tem certeza?"
  ├─ Se Confirmar:
  │  ├─ Executar DeleteOperation command
  │  ├─ Recarregar tabela
  │  └─ Exibir Snackbar "Deletado"
  └─ Se Cancelar: fechar dialog
```

---

## ✨ Melhorias Futuras (Out of Scope)

```
[ ] Paginação de operações (>50)
[ ] Busca/filtro de operações
[ ] Operações com permissionamento granular
[ ] Importação em massa
[ ] Histórico de auditorias
[ ] API REST para operações
[ ] Temas escuro/claro
[ ] Exportar operações (CSV/JSON)
```

---

## 📞 Se Tiver Dúvida...

1. **Requisitos** → Leia `OPERATIONS_FEATURE_PLAN.md`
2. **Código** → Consulte `OPERATIONS_TECHNICAL_GUIDE.md`
3. **Visual** → Veja `OPERATIONS_VISUAL_DESIGN.md`
4. **Estimativas** → Check `OPERATIONS_SUMMARY.md`

---

## ✅ Antes de Terminar

```
[ ] Todos os testes passam
[ ] Build sem warnings
[ ] Code review feito
[ ] Manual testing de todos fluxos
[ ] Responsividade validada
[ ] Performance aceitável
[ ] Mensagens amigáveis
[ ] Documentação atualizada
[ ] Commit e push para branch dev_functions_operations
```

---

**Estado atual:** a implementação está no código; o teste manual da UI e a aplicação de `IncreaseOperationFriendlyIdLength` ainda estão pendentes. Consulte `IMPLEMENTATION_SUMMARY.md` antes de executar a validação final.
