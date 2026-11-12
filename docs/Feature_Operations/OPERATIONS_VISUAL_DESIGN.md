# Especificação Visual e UX - Feature Operations

## 1. Wireframes e Layouts

### 1.1 Listagem de Funcionalidades (FunctionalitiesList.razor) - Modificação

```
┌─────────────────────────────────────────────────────────────────────────┐
│ Functionalities                                          [+ New]         │
├─────────────────────────────────────────────────────────────────────────┤
│ Application │ Module  │ Icon │ Name           │ Abbr  │ Active │ Actions│
├─────────────────────────────────────────────────────────────────────────┤
│ App 1       │ Module1 │ 📦   │ Users          │ USR   │   ✓    │ ⚙️ ✏️ 🗑 │
│ App 1       │ Module1 │ 📦   │ Products       │ PRD   │   ✓    │ ⚙️ ✏️ 🗑 │
│ App 2       │ Module2 │ 📦   │ Reports        │ RPT   │   ✗    │ ⚙️ ✏️ 🗑 │
└─────────────────────────────────────────────────────────────────────────┘

LEGENDA BOTÕES:
  ⚙️  = Novo: "Gerenciar Operações" (Tune icon, Primary color)
  ✏️  = Edit (Info color)
  🗑  = Delete (Error color)
```

### 1.2 Modal de Gerenciamento de Operações (OperationsModal.razor)

```
╔════════════════════════════════════════════════════════════════════════════╗
║ 📋 Gerenciar Operações: Users                              [X]              ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║ ┌─ Operações Padrão ────────────────────────────────────────────────────┐  ║
║ │                                                                        │  ║
║ │  [✔ Adicionar Operações Padrão]  (Success/Green color)              │  ║
║ │                                                                        │  ║
║ └────────────────────────────────────────────────────────────────────────┘  ║
║                                                                              ║
║ ┌─ Operações Cadastradas ────────────────────────────────────────────────┐  ║
║ │                                                                        │  ║
║ │ Name    │ Code   │ Friendly ID │ Active │     Actions               │  ║
║ ├─────────┼────────┼─────────────┼────────┼─────────────────────────────┤  ║
║ │ View    │ VIEW   │ view        │  ✓     │ ⭕ ✏️  🗑                     │  ║
║ │ Create  │ CREATE │ create      │  ✓     │ ⭕ ✏️  🗑                     │  ║
║ │ Update  │ UPDATE │ update      │  ✓     │ ⭕ ✏️  🗑                     │  ║
║ │ Delete  │ DELETE │ delete      │  ✗     │ ⭕ ✏️  🗑                     │  ║
║ │ Export  │ EXPORT │ export      │  ✓     │ ⭕ ✏️  🗑                     │  ║
║ │                                                                        │  ║
║ └────────────────────────────────────────────────────────────────────────┘  ║
║                                                                              ║
║ ┌─ Form (Expandible) ───────────────────────────────────────────────────┐  ║
║ │  [HIDDEN - expands when Edit or Create clicked]                     │  ║
║ └────────────────────────────────────────────────────────────────────────┘  ║
║                                                                              ║
║ ┌────────────────────────────────────────────────────────────────────────┐  ║
║ │                                [Cancelar]  [Fechar]                   │  ║
║ └────────────────────────────────────────────────────────────────────────┘  ║
║                                                                              ║
╚════════════════════════════════════════════════════════════════════════════╝

LEGENDA:
  ⭕ = Toggle Ativar/Desativar
       - Ativo (✓):   Ícone Warning (laranja)
       - Inativo (✗): Ícone Tertiary (cyan)
  ✏️  = Editar (Info/cyan)
  🗑  = Deletar (Error/vermelho)
```

### 1.3 Formulário de Criar/Editar Operação (OperationForm.razor)

```
┌──────────────────────────────────────────────────────────┐
│ Operation Name                                           │
│ [________________________________]                      │
│                                                          │
│ Code                   │ Friendly ID                    │
│ [________________]     │ [________________]              │
│                                                          │
│ ⚠️ Código já existe. Por favor, use outro.              │
│                                                          │
│                            [Cancelar]  [Salvar]         │
└──────────────────────────────────────────────────────────┘

MODOS:
  - Create: Campos vazios, título "Nova Operação"
  - Edit: Campos preenchidos, título "Editar Operação: [Name]"

VALIDAÇÕES EM TEMPO REAL:
  - Código único globalmente
  - FriendlyId único globalmente
  - Campos obrigatórios marcados com *
```

### 1.4 Dialog de Confirmação (Deletar)

```
╔════════════════════════════════════════════════════════════╗
║ ⚠️  Confirm Delete                                    [X]  ║
╠════════════════════════════════════════════════════════════╣
║                                                            ║
║  Are you sure you want to delete the operation 'View'?    ║
║                                                            ║
║  ┌────────────────────────────────────────────────────┐  ║
║  │                  [Cancel]  [Delete]                │  ║
║  └────────────────────────────────────────────────────┘  ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝

CORES:
  - Botão Delete: Color.Error (vermelho)
  - Ícone: Icons.Material.Filled.Warning (amarelo)
```

---

## 2. Estados e Transições

### 2.1 Estados do Modal

```
                    ┌─────────────────┐
                    │  Modal Loading  │
                    │   (spinner)     │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
        ┌──────────►│  Operations     │◄──────────┐
        │           │   Loaded        │           │
        │           └────────┬────────┘           │
        │                    │                    │
    Reload          Edit/Create/Delete        Reload
        │                    │                    │
        │                    ▼                    │
        │           ┌─────────────────┐           │
        └───────────┤   Form Open     ├───────────┘
                    │  (Create/Edit)  │
                    └─────────────────┘
```

### 2.2 Estados do Formulário

```
┌──────────────┐
│   Initial    │
│   (Empty)    │
└────────┬─────┘
         │
         ▼
┌──────────────────┐
│  User Typing     │
│ (Real-time Validation) │
└────────┬─────────┘
         │
    ┌────┴─────┐
    │           │
    ▼           ▼
┌──────────┐ ┌──────────┐
│ Valid    │ │ Invalid  │
│ (Save OK)│ │ (Error)  │
└──────────┘ └──────────┘
```

---

## 3. Cores e Estilos

### 3.1 Paleta MudBlazor Utilizada

| Componente | Cor MudBlazor | Hex Value | RGB | Uso |
|---|---|---|---|---|
| Botão Padrão | Primary | #1976d2 | rgb(25, 118, 210) | Ações gerais |
| Botão Sucesso | Success | #4caf50 | rgb(76, 175, 80) | Criar, Adicionar |
| Botão Info | Info | #00bcd4 | rgb(0, 188, 212) | Editar |
| Botão Erro | Error | #f44336 | rgb(244, 67, 54) | Deletar |
| Botão Warning | Warning | #ff9800 | rgb(255, 152, 0) | Ativo |
| Botão Tertiary | Tertiary | #00bcd4 | rgb(0, 188, 212) | Inativo |
| Botão Secundário | Secondary | #757575 | rgb(117, 117, 117) | Cancelar |

### 3.2 Typography

```
Títulos Modais:     Typo.h6 (font-weight: 600)
Subtítulos:         Typo.subtitle2 (font-weight: 600)
Labels de Tabela:   font-weight: bold
Texto Normal:       Typo.body2
Links:              Typo.body2 com Color.Primary
Erros:              Typo.caption com Color.Error
```

### 3.3 Espaçamento

```
Padding Modal:      8px
Margin Seções:      24px (bottom)
Margin Campos:      16px (bottom)
Gap entre Botões:   12px
Border Radius:      4px (padrão MudBlazor)
```

---

## 4. Componentes MudBlazor Utilizados

### 4.1 OperationsModal.razor

| Componente | Props | Descrição |
|---|---|---|
| `MudDialog` | - | Container do modal |
| `MudText` | Typo, GutterBottom | Títulos e labels |
| `MudDivider` | Class | Separadores |
| `MudButton` | Variant, Color, OnClick | Botões de ação |
| `MudIcon` | Icon, Color, Title | Ícones |
| `MudTable` | Items, Dense, Hover | Tabela de dados |
| `MudProgressLinear` | Indeterminate, Color | Loading spinner |
| `MudAlert` | Severity | Mensagens inline |

### 4.2 OperationForm.razor

| Componente | Props | Descrição |
|---|---|---|
| `EditForm` | Model | Form container |
| `DataAnnotationsValidator` | - | Validação de dados |
| `MudTextField` | Label, Variant, FullWidth | Campo de texto |
| `MudGrid` | - | Layout grid |
| `MudItem` | md | Coluna responsiva |
| `MudButton` | Type, Variant, Color | Botões submit/cancel |

### 4.3 FunctionalitiesList.razor (Modificação)

```razor
<!-- Adicionar na seção de Actions dentro da MudTable -->
<MudButton Size="Size.Small" 
           Variant="Variant.Outlined" 
           Color="Color.Primary" 
           OnClick="() => OpenOperationsModal(context.Id, context.Name)"
           Class="mr-1">
    <MudIcon Icon="@Icons.Material.Filled.Tune" 
             Color="Color.Primary" 
             Title="Manage Operations" />
</MudButton>
```

---

## 5. Ícones Utilizados

### Material Design Icons (MudBlazor)

```csharp
// Cabeçalho Modal
Icons.Material.Filled.Assignment        // 📋 (principal)

// Botões de Ação
Icons.Material.Filled.Tune               // ⚙️ (Gerenciar Operações)
Icons.Material.Filled.CheckCircle        // ✔ (Ativar/Desativar)
Icons.Material.Filled.Edit                // ✏️ (Editar)
Icons.Material.Filled.Delete              // 🗑 (Deletar)
Icons.Material.Filled.Add                 // ➕ (Novo)

// Feedback
Icons.Material.Filled.Warning            // ⚠️ (Aviso/Confirmação)
Icons.Material.Filled.CheckCircleOutline // ✔ (Sucesso)

// Loading
Material Progress Circular               // Spinner animado
```

---

## 6. Feedback Visual - Snackbars

### Sucesso (Severity.Success - Verde)

```
┌─────────────────────────────────────────────────────┐
│ ✔ Operation created successfully!              [X]   │
└─────────────────────────────────────────────────────┘
Duração: 3 segundos

Mensagens:
- "Operation created successfully!"
- "Operation updated successfully!"
- "Operation deleted successfully!"
- "3 standard operation(s) added successfully!"
- "Operation activated!"
- "Operation deactivated!"
```

### Aviso (Severity.Warning - Laranja)

```
┌─────────────────────────────────────────────────────┐
│ ⚠ Some standard operations already exist.    [X]    │
└─────────────────────────────────────────────────────┘
Duração: 4 segundos
```

### Erro (Severity.Error - Vermelho)

```
┌─────────────────────────────────────────────────────┐
│ ✕ Operation code must be unique.              [X]   │
└─────────────────────────────────────────────────────┘
Duração: 5 segundos

Mensagens:
- "Operation code must be unique."
- "Operation friendly ID must be unique."
- "The specified functionality does not exist."
- "Error creating operation: [details]"
- "Error updating operation: [details]"
- "Error deleting operation: [details]"
```

### Informação (Severity.Info - Cyan)

```
┌─────────────────────────────────────────────────────┐
│ ℹ No operations found for this functionality. [X]   │
└─────────────────────────────────────────────────────┘
```

---

## 7. Estados dos Botões

### Botão "Adicionar Operações Padrão"

```
┌─ Estado Normal ──────────────────────────────────┐
│ [✔ Adicionar Operações Padrão]                 │
│ Color: Success, Variant: Filled, Enabled        │
└──────────────────────────────────────────────────┘

┌─ Estado Desabilitado (todas ops padrão existem) ┐
│ [✔ Adicionar Operações Padrão] (disabled)      │
│ Color: Success, Variant: Filled, Disabled       │
│ Opacidade: 50%                                   │
└──────────────────────────────────────────────────┘

┌─ Estado Loading ─────────────────────────────────┐
│ [⟳ Adicionar Operações Padrão]                 │
│ Color: Success, Variant: Filled, Disabled       │
│ Com spinner (MudProgressCircular)                │
└──────────────────────────────────────────────────┘
```

### Botão "Ativar/Desativar"

```
ATIVO:
  ⭕ [CheckCircle Icon]
  Color: Warning (Laranja)
  Tooltip: "Disable"
  Size: Small
  Variant: Outlined

INATIVO:
  ⭕ [CheckCircle Icon]
  Color: Tertiary (Cyan)
  Tooltip: "Enable"
  Size: Small
  Variant: Outlined
```

---

## 8. Responsividade

### Desktop (≥960px)

```
Modal Width: 600px
Table Columns: 5 colunas visíveis
Form: Grid de 12 colunas
  - Name: 12 colunas
  - Code: 6 colunas | FriendlyId: 6 colunas
```

### Tablet (600px - 959px)

```
Modal Width: 90% da tela
Table Columns: 4 colunas (Actions colapsadas)
Form: Grid de 12 colunas (responsivo)
Fonte: 1rem
```

### Mobile (<600px)

```
Modal Width: 100% da tela
Modal MaxHeight: 90vh
Table: Versão compacta (2 colunas + Actions)
Form: Grid simples (1 coluna)
Fonte: 0.875rem
Padding: Reduzido para 4px
```

---

## 9. Acessibilidade

### ARIA Labels

```razor
<MudButton 
    aria-label="Manage operations for Users"
    title="Manage Operations">
    <MudIcon Icon="@Icons.Material.Filled.Tune" />
</MudButton>

<MudButton 
    aria-label="Toggle operation status"
    title="Toggle Status">
    <MudIcon Icon="@Icons.Material.Filled.CheckCircle" />
</MudButton>
```

### Teclado

- Tab: Navega entre botões
- Enter: Ativa botão/abre formulário
- Escape: Fecha modal
- Ctrl+S: Salva formulário (se implementado)

### Contraste

- Todas as cores escolhidas atendem WCAG AA (4.5:1 mínimo)
- Ícones combinados com texto/cor para indicação
- Estados claros (não apenas cor)

---

## 10. Animações e Transições

### Abrir Modal

```
Duração: 300ms
Easing: EaseInOut
Propriedades:
  - Opacity: 0 → 1
  - Transform: scale(0.9) → scale(1)
```

### Carregar Dados

```
Spinner Animation: Rotação contínua
Duração: 1s (loop)
Exibir enquanto _isLoading = true
```

### Hover em Botões

```
Duração: 200ms
Propriedades:
  - Background: 5% overlay
  - Cursor: pointer
```

### Transição de Tabela

```
Ao adicionar nova linha:
  - Fade in: 300ms
  - Slide: translateY(-10px) → translateY(0)
```

---

## 11. Tema Escuro (Futuro)

Preparar para suporte a tema escuro:

```css
/* Light Theme (padrão) */
.operations-modal-container {
    background-color: #ffffff;
    color: rgba(0, 0, 0, 0.87);
}

/* Dark Theme (futuro) */
@media (prefers-color-scheme: dark) {
    .operations-modal-container {
        background-color: #1e1e1e;
        color: #ffffff;
    }
}
```

---

## 12. Exemplos de Mensagens

### Criar Operação com Sucesso

```
Ação: Usuário clica "Adicionar Operação", preenche campos, clica "Salvar"
Resultado: Snackbar verde "Operation created successfully!" (3s)
Modal: Tabela recarrega, nova linha aparece com fade-in
```

### Erro - Código Duplicado

```
Ação: Usuário tenta criar operação com Code = "VIEW" (já existe)
Erro: Snackbar vermelho "Operation code must be unique."
Form: Campo Code destacado em vermelho, mensagem inline
```

### Adicionar Operações Padrão com Sucesso

```
Ação: Usuário clica "Adicionar Operações Padrão" (nenhuma existe)
Resultado: Snackbar "4 standard operation(s) added successfully!"
Modal: Tabela recarrega, 4 novas linhas aparecem
Botão: Desabilitado ("Adicionar Operações Padrão" está grayed out)
```

### Deletar com Confirmação

```
Ação: Usuário clica ícone Deletar na linha "View"
Dialog: "Are you sure you want to delete the operation 'View'?"
Se Confirmar: Snackbar "Operation deleted successfully!", linha desaparece
Se Cancelar: Dialog fecha, nada acontece
```

---

## 13. Checklist Visual/UX

- [ ] Modal responde em 300ms
- [ ] Ícones visíveis e intuitivos
- [ ] Cores consistentes com paleta MudBlazor
- [ ] Snackbars aparecem no canto inferior-direito
- [ ] Tabela horizontal sem scroll em desktop
- [ ] Responsivo em mobile (testar em 375px)
- [ ] Confirmação clara antes de deletar
- [ ] Feedback visual durante loading
- [ ] Erros mostram causa específica
- [ ] Botões desabilitados durante processamento
- [ ] Espaçamento consistente (16px base)
- [ ] Tipografia hierárquica clara
- [ ] Hover states visíveis
- [ ] Focus states acessíveis (outline)
- [ ] Sem conteúdo truncado em mobile

---

**Documento Visual Completo - Pronto para Design/Desenvolvimento**
