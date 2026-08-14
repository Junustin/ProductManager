using ProductManager.Component.CartComponents;
using ProductManager.Interface;
using ProductManager.Services;

namespace ProductManager.Command
{
	public class AddToCartCommand : ICommand
	{
		public string Name => "AddToCart";

		public string Description => "Add product to cart";

		private readonly IProductRepository _productRepository;
		private readonly ShoppingCart _cart;


		public AddToCartCommand(IProductRepository productRepository, ShoppingCart shoppingCart)
		{
			_productRepository = productRepository;
			_cart = shoppingCart;
		}

		public void Execute(string[] args)
		{
			if(args.Length != 2)
			{
				ConsoleLogger.LogError("Invalid syntax! Usage: AddToCart <ProductId> <Quantity>");
				return;
			}

			if(!int.TryParse(args[0], out int id))
			{
				ConsoleLogger.LogError("Product ID must be positive number");
				return;
			}

			if (!int.TryParse(args[1], out int quantity))
			{
				ConsoleLogger.LogError("Quantity must be positive number");
				return;
			}

			// Fetch product from database
			var product = _productRepository.GetById(id);

			if (product == null)
			{
				ConsoleLogger.LogError($"No product with ID: {id} found.");
				return;
			}

			// If has same product already in cart add up the quantity. 
			var existingInCart = _cart.Items.FirstOrDefault(i => i.ProductId == id);
			int cartQuantity = existingInCart?.Quantity ?? 0;
			int totalRequested = cartQuantity + quantity;

			// Check stock if has enough to satisfy user input
			if(totalRequested > product.Stock)
			{
				ConsoleLogger.LogError($"Cannot add {quantity} {product.Name} to card. Avaliable stock: {product.Stock} Request: {totalRequested}");
				return;
			}

			// Add item to cart
			_cart.AddItem(new CartItem
			{
				ProductId = product.Id,
				Name = product.Name,
				Quantity = quantity,
				UnitPrice = product.Price
			});

			ConsoleLogger.LogSuccess($"Add [bold cyan]{quantity}x {product.Name}[/] to shopping cart.");
		}
	}
}
