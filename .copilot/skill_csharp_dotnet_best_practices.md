# Skill: C#/.NET Best Practices for 3ASystem

Esta skill garante que todo código implementado siga as melhores práticas de C#/.NET, considerando a arquitetura e padrões adotados na solução 3ASystem.

## 1. Organização e Arquitetura
- Siga Clean Architecture: separe Presentation, Application, Domain e Infrastructure.
- Use CQRS para separar comandos e queries.
- Centralize regras de negócio no domínio.
- Use injeção de dependência para todos os serviços e repositórios.

## 2. Padrões de Código C#
- Use nomes claros, descritivos e em inglês (PascalCase para tipos/métodos, camelCase para variáveis).
- Prefira `record` para value objects e DTOs imutáveis.
- Use propriedades automáticas e expression-bodied members quando possível.
- Utilize pattern matching e nullability annotations.
- Siga SOLID e DRY.

## 3. Práticas para Blazor
- Separe lógica de UI em arquivos `.razor.cs` (code-behind).
- Use MudBlazor para componentes visuais.
- Evite lógica de negócio em componentes Blazor.
- Prefira parâmetros e cascading parameters para compartilhamento de estado.

## 4. Testabilidade
- Escreva testes unitários para lógica de domínio e aplicação.
- Use xUnit, FluentAssertions e NSubstitute para testes.
- Mantenha alta cobertura de código para paths críticos.

## 5. Validação e Segurança
- Use FluentValidation para validação de entrada e regras de negócio.
- Valide todos os dados de entrada.
- Use ASP.NET Core Identity para autenticação/autorização.
- Siga o princípio do menor privilégio para permissões.

## 6. Logging e Diagnóstico
- Use Serilog para logging estruturado.
- Logue informações relevantes em níveis apropriados (Information, Warning, Error).

## 7. Eficiência e Performance
- Use async/await para operações I/O.
- Evite consultas N+1 no Entity Framework.
- Prefira consultas projetadas (Select) para evitar carregamento desnecessário.

---

Esta skill deve ser aplicada a todo novo código ou refatoração na solução 3ASystem para garantir qualidade, manutenibilidade e alinhamento arquitetural.
