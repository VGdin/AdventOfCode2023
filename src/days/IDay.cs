namespace AdventOfCode.src.days
{
    public interface IDay
    {

        public string Solution { get; }
        public string Name { get; }
        
        void Solve();
    }
}
