using System.ComponentModel.DataAnnotations;

namespace GoldCube
{
    public class Cell()
    {
        [Key]
        public int Value { get; set; } // 0-36
        public Colors Color { get; set; }
        public bool IsEven { get; set; }
        public Sectors Sector { get; set; } // 1-12 ...

        public static Colors GetColor(int num)
        {
            if (num == 0) return Colors.Green;
            else if ((num >= 1 && num <= 10) || (num >= 19 && num <= 28))
            {
                return num % 2 == 0 ? Colors.Black : Colors.Red;
            }
            else
                return num % 2 != 0 ? Colors.Black : Colors.Red;
        }
    }
}