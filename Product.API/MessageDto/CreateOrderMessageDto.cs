namespace Product.API.MessageDto
{
    /// <summary>
    /// 下單事件消息
    /// </summary>
    public class CreateOrderMessageDto
    {
        /// <summary>
        /// 產品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 購買數量
        /// </summary>
        public int Count { get; set; }
    }
}
