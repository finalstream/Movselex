namespace FinalstreamCommons.Frameworks
{
    /// <summary>
    /// �W���̃A�N�V������\���܂��B
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGeneralAction<T>
    {
        /// <summary>
        /// �A�N�V���������s���܂��B
        /// </summary>
        /// <param name="param"></param>
        void Invoke(T param);
    }
}