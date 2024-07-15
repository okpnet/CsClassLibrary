namespace ProgressArgments
{
    /// <summary>
    /// 進捗率インターフェイス
    /// </summary>
    public interface IProgressArg
    {
        /// <summary>
        /// 進捗パーセント
        /// </summary>
        int ProgressPercent { get; }
    }
    /// <summary>
    /// 任意の型のオブジェクト有り進捗率インターフェイス
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface IProgressArg<TItem> : IProgressArg
    {
        TItem? Item { get; }
    }
    /// <summary>
    /// メッセージインターフェイス
    /// </summary>
    public interface IProgressMessage
    {        
        /// <summary>
        /// ラベル
        /// </summary>
        string Lable { get; }
        /// <summary>
        /// メッセージ
        /// </summary>
        string Message { get; }
    }
    /// <summary>
    /// 進捗引数
    /// </summary>
    public class ProgressArg: IProgressArg
    {
        /// <summary>
        /// 進捗パーセント
        /// </summary>
        public int ProgressPercent=> MaxCount == 0 || ProgressCount == 0 ? 0 : (int)Math.Floor((decimal)(ProgressCount * 100) / (decimal)(MaxCount));
        /// <summary>
        /// 進捗最大
        /// </summary>
        public uint MaxCount { get; }
        /// <summary>
        /// 現在地
        /// </summary>
        public uint ProgressCount { get; set; } = 0;


        public ProgressArg(uint maxCount)
        {
            MaxCount = maxCount;
        }
    }
    /// <summary>
    /// メッセージ
    /// </summary>
    public class ProgressMessageArg:ProgressArg,IProgressArg,IProgressMessage
    {
        /// <summary>
        /// ラベル
        /// </summary>
        public string Lable { get; set; } = string.Empty;
        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public ProgressMessageArg(uint maxCount):base(maxCount) 
        {
        }
    }
    /// <summary>
    /// 任意の型のオブジェクト有り進捗率
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class ProgressArg<TItem> : ProgressArg, IProgressArg,IProgressArg<TItem>
    {
        /// <summary>
        /// オブジェクト
        /// </summary>
        public TItem? Item { get; set; }
        public ProgressArg(uint maxCount,TItem? item) : base(maxCount)
        {
            Item = item;
        }
    }
    /// <summary>
    /// 任意の型のオブジェクト，メッセージ有り進捗率
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class ProgressMessageArg<TItem>:ProgressMessageArg,IProgressArg,IProgressMessage,IProgressArg<TItem>
    {
        /// <summary>
        /// オブジェクト
        /// </summary>
        public TItem? Item { get; }

        public ProgressMessageArg(uint maxCount, TItem? item) : base(maxCount)
        {
            Item = item;
        }
    }
}
