namespace ServerlessSleepless.Awaker.Abstraction
{
    public interface ISerializer
    {
        string Serialize(object o);

        T Deserialize<T>(string s) where T: class;
    }
}
