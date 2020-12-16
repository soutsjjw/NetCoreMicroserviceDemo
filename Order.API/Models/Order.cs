using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models
{
    /// <summary>
    /// 訂單實體類別
    /// </summary>
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 下單時間
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 產品ID
        /// </summary>
        [Required]
        public int ProductID { get; set; }

        /// <summary>
        /// 購買數量
        /// </summary>
        [Required]
        public int Count { get; set; }
    }
}
