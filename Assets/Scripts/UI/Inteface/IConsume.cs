public interface IConsume {
    string ItemName{get;set;}
    int ConsumeCount{get;set;}
    void PreConsume();
    bool PostConsume();
    void AfterConsume();
}