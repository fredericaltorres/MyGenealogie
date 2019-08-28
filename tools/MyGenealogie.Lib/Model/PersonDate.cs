namespace MyGenealogie
{
    public class PersonDate {
        public int Year;
        public int Month;
        public int Day;

        public PersonDate()
        {

        }

        public PersonDate SetToNow()
        {
            var d = System.DateTime.Now;
            this.Year = d.Year;
            this.Month = d.Month;
            this.Day = d.Day;

            return this;
        }
    }
}


