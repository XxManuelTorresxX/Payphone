using Payphone.Domain.Entities;
using Xunit;

public class OrderTest
{
    [Fact]
    public void ChangeStatus_Should_ThrowException_When_SkippingStates()
    {        
        var order = new Order("customer123", 100m);
             
        Assert.Throws<InvalidOperationException>(() =>
            order.ChangeStatus(OrderStatus.Entregado)
        );
    }

    [Fact]
    public void UpdateTotalAmount_Should_ThrowException_When_AmountIsZeroOrNegative()
    {        
        var order = new Order("customer123", 50m);
             
        Assert.Throws<ArgumentException>(() =>
            order.UpdateTotalAmount(0)
        );
    }
}
