using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.API.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        [Required]
        [Column(TypeName = "VARCHAR(16)")]
        public string Name { get; set; }

        /// <summary>
        /// 庫存
        /// </summary>
        [Required]
        public int Stock { get; set; }
    }
}
