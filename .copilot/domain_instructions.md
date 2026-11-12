# Domain Project Instructions

Estas instruções devem ser seguidas sempre que novas entidades ou arquivos compartilhados forem criados no projeto de domínio (`3ASystem.Domain`).

## 1. Organização e Estrutura
- Coloque entidades em `Entities/<Contexto>` (ex: `Entities/Modules/Module.cs`).
- Coloque value objects, enums e tipos compartilhados em `Shared/`.
- Coloque interfaces de domínio em `Abstractions/`.

## 2. Padrões de Código
- Use classes imutáveis para value objects (preferencialmente `record`).
- Entidades devem herdar de uma base (`Entity` ou similar) e implementar identificador único.
- Use eventos de domínio para mudanças relevantes no estado da entidade.
- Siga convenções de nomenclatura: nomes claros, em inglês, PascalCase.

## 3. Princípios de DDD
- Mantenha lógica de negócio/coerência dentro das entidades.
- Não inclua dependências de infraestrutura, UI ou aplicação.
- Use métodos de fábrica estáticos para criação de entidades complexas.
- Valide invariantes no construtor ou métodos de criação.

## 4. Compartilhamento
- Tipos compartilhados (erros, resultados, enums, value objects) devem ser colocados em `Shared/`.
- Não duplique tipos entre entidades; centralize o que for comum.

## 5. Testabilidade
- Escreva testes unitários para entidades e value objects.
- Garanta que invariantes e regras de negócio sejam cobertos por testes.

## 6. Documentação
- Documente regras de negócio relevantes em comentários XML nas entidades e value objects.

---

Estas instruções garantem consistência, manutenibilidade e alinhamento com DDD no projeto de domínio.
