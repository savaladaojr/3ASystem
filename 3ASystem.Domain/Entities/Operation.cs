using _3ASystem.Domain.Abstractions;
using _3ASystem.Domain.Entities.Identifiers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace _3ASystem.Domain.Entities
{
	public sealed class Operation : Entity<OperationId>
	{
		public FunctionalityId FunctionalityId { get; private set; } = default!;

		public string Name { get; private set; } = default!;
		public string Code { get; private set; } = default!;
		public bool IsActive { get; private set; } = true;

		//EF Relations
		public Functionality? Functionality { get; init; }

		private Operation()
		{
		}

		private Operation(OperationId id, FunctionalityId functionalityId, string name, string code, bool isActive) : base(id)
		{
			FunctionalityId = functionalityId;
			Name = name;
			Code = code;
			IsActive = isActive;
		}

		public static Operation Create(FunctionalityId functionalityId, string name, string code)
		{
			var action = new Operation
			(
				new OperationId(Guid.NewGuid()),
				functionalityId,
				name,
				code,
				true
			);

			return action;
		}

		public void Update(string name, string code)
		{
			Name = name;
			Code = code;
			LastUpdatedAt = DateTime.Now;
		}

		public void Enable()
		{
			IsActive = true;
			LastUpdatedAt = DateTime.Now;
		}

		public void Disable()
		{
			IsActive = false;
			LastUpdatedAt = DateTime.Now;
		}


	}

}
