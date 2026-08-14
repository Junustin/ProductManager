namespace ProductManager.Component.OrderComponents
{
	public class Order
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public decimal TotalAmount { get; set; }

		// Navigation property for in-memory handling
		public List<OrderItem> Items { get; set; } = new();
	}

	public class OrderItem
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; } // Price snapshot
		public decimal LineTotal => Quantity * UnitPrice; // Derived property
	}
}
