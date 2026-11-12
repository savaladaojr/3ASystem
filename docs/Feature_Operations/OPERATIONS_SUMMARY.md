# Sumário Executivo - Feature: Operations Management

## Visão Geral
Implementar um sistema completo para gerenciar **Operações** de **Funcionalidades** através de um modal intuitivo que permite CRUD, ativação/desativação e adição automática de operações padrão (View, Create, Update, Delete).

---

## Objetivo
Permitir que administradores gerenciem as operações (ações/permissões) disponíveis para cada funcionalidade do sistema de forma centralizada e fácil.

---

## Escopo

### ✅ Incluso
- Modal de gerenciamento de operações acessível via botão na listagem de funcionalidades
- Tabela com listagem de operações cadastradas (Name, Code, FriendlyId, IsActive)
- CRUD completo de operações (Create, Read, Update, Delete)
- Toggle ativar/desativar operação
- Botão para adicionar operações padrão (View, Create, Update, Delete) com validação de duplicatas
- Confirmação de exclusão
- Feedback visual (Snackbars) para todas as ações
- Validações de negócio (código único, friendly ID único)

### ❌ Fora do Escopo (Future)
- Operações com permissionamento granular
- Importação em massa de operações
- Histórico de auditorias
- API REST para operações (foco UI)

---

## Arquitetura
Segue os padrões clean architecture já estabelecidos no projeto:

```
┌─────────────────────────────────────┐
│         UI Layer (Razor)             │
│  OperationsModal / OperationForm     │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│    Application Layer (MediatR)       │
│  Commands/Queries & Handlers         │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│      Domain Layer (Entities)         │
│  Operation / OperationErrors         │
└────────────┬────────────────────────┘
             │
┌────────────▼────────────────────────┐
│  Infrastructure (Repository/EF)      │
│  OperationRepository                 │
└─────────────────────────────────────┘
```

---

## Tecnologias
- **Framework UI**: MudBlazor (Material Design)
- **Padrão**: Clean Architecture + CQRS + Repository
- **Orquestração**: MediatR
- **Persistência**: Entity Framework Core
- **Validação**: FluentValidation

---

## Componentes Principais

### 1. **OperationsModal.razor**
- Modal principal que encapsula todo o gerenciamento
- Tabela de operações existentes
- Seção de operações padrão
- Carregamento lazy de dados

### 2. **OperationForm.razor**
- Formulário reutilizável (criar/editar)
- Validações de código/friendlyId únicos
- Modos: Create | Edit

### 3. **FunctionalitiesList.razor (modificado)**
- Botão "Gerenciar Operações" em cada linha
- Abre OperationsModal passando FunctionalityId

### 4. **Handlers de Negócio**
- `CreateOperationCommandHandler`
- `UpdateOperationCommandHandler`
- `DeleteOperationCommandHandler`
- `EnableDisableOperationCommandHandler`
- `GetOperationsByFunctionalityHandler`

---

## Fluxo Crítico - Adicionar Operações Padrão

```
┌─────────────────────────────────────┐
│ Usuário clica "Adicionar Operações  │
│          Padrão"                    │
└────────────┬────────────────────────┘
             │
             ▼
    ┌────────────────────┐
    │ Validar: verificar │
    │ se View/Create/    │
    │ Update/Delete já   │
    │ existem            │
    └────┬───────────────┘
         │
    ┌────▼──────────────┐         ┌──────────────────┐
    │ Se tem            │         │ Se tudo OK:      │
    │ duplicatas?       │         │ Criar 4 ops em   │
    │ → Snackbar aviso  │────No───►transação         │
    │                   │         └────┬─────────────┘
    │ Yes              │              │
    │                   │              ▼
    └──────────────────┘    ┌──────────────────────┐
                            │ Reload lista + Show  │
                            │ "Sucesso!" Snackbar  │
                            │ Disable botão        │
                            └──────────────────────┘
```

---

## Paleta de Cores Aplicada

| Ação | Cor | Significado |
|------|-----|------------|
| Gerenciar / Novo | Primary (Azul) | Ação principal |
| Adicionar Padrão | Success (Verde) | Criação |
| Editar | Info (Cyan) | Informativo |
| Deletar | Error (Vermelho) | Destrutivo |
| Ativar | Warning (Laranja) | Status ativo |
| Desativar | Tertiary (Cyan) | Status inativo |
| Cancelar | Secondary | Secundário |

---

## Validações de Negócio

### Criar Operação
- ✓ Name: obrigatório, máx 100 caracteres
- ✓ Code: obrigatório, máx 50, **ÚNICO** globalmente
- ✓ FriendlyId: obrigatório, máx 50, **ÚNICO** globalmente
- ✓ FunctionalityId deve existir

### Editar Operação
- ✓ Mesmas validações de criação
- ✓ Code/FriendlyId únicos (excluindo registro atual)

### Operações Padrão
- ✓ Manter operações padrão existentes e criar somente as faltantes
- ✓ Colisões são detectadas antes de inserir; `SaveChanges` persiste inserções atomicamente

### Deletar Operação
- ✓ Validar se há RoleOperations referenciando (future)
- ✓ Retornar erro se há dependências

---

## Estimativa

| Fase | Horas | Descrição |
|------|-------|-----------|
| Domain Layer | 1.5h | Erros de validação, regras de negócio |
| Application Layer | 3.5h | 4 Commands + 2 Queries + Validators |
| UI Components | 5h | OperationsModal, OperationForm, Lógica |
| Integração | 2.5h | DI, FunctionalitiesList, Testes e/e |
| Tests | 2.5h | Unit + Integration + Manual |
| Refinements | 1.5h | UX, CSS, Mensagens |
| **Total** | **~16.5h** | Estimado |

---

## Checklist de Entrega

### Funcionalidades
- [ ] Botão "Gerenciar Operações" em cada linha da tabela
- [ ] Modal abre com listagem de operações
- [ ] Criar nova operação (formulário + validação)
- [ ] Editar operação existente
- [ ] Deletar operação com confirmação
- [ ] Ativar/desativar operação (toggle)
- [ ] Botão "Adicionar Operações Padrão"
  - [ ] Validação de duplicatas
  - [ ] Adição automática de View/Create/Update/Delete
  - [x] Reexecução segura quando todas ou algumas já existem
- [ ] Feedback visual (Snackbars + Dialogs)

### Qualidade
- [ ] Validações de negócio implementadas
- [ ] Unit tests para commands/queries
- [ ] Integration tests para handlers
- [ ] Manual testing (todos os fluxos)
- [ ] Responsividade em mobile/tablet
- [ ] Tratamento de erros robusto
- [ ] Mensagens de erro clareza

### Código
- [ ] Segue padrões do projeto
- [ ] Nomeação consistente
- [ ] Sem código duplicado
- [ ] Comentários onde necessário
- [ ] Performance aceitável

---

## Riscos e Mitigações

| Risco | Probabilidade | Impacto | Mitigação |
|-------|--------------|--------|-----------|
| Validação de código/friendlyId única não funciona | Baixa | Médio | Testar com dados existentes |
| Modal muito lento com muitas operações | Baixa | Baixo | Adicionar paginação se >50 ops |
| Usuário deleta operação crítica | Média | Alto | Confirmação + Soft delete |
| Erro no adicionar padrão deixa inconsistência | Baixa | Alto | Usar transação + rollback |

---

## Próximos Passos

1. ✅ **Planejamento** (documento)
2. ✅ **Implementação e testes automatizados** (65 testes do projeto Application passaram)
3. ⏳ **Aplicar migração** `IncreaseOperationFriendlyIdLength`
4. ⏳ **Validação manual** dos fluxos da UI
5. ⏳ **Revisão, merge e deploy** (staging → production)

---

## Referências

- Documento detalhado: `src\docs\OPERATIONS_FEATURE_PLAN.md`
- Repo: `https://github.com/savaladaojr/3ASystem`
- Branch: `dev_functions_operations`

---

**Data**: Março 2025  
**Versão**: 1.0 - Planejamento  
**Status**: Implementado; migração e validação manual pendentes
