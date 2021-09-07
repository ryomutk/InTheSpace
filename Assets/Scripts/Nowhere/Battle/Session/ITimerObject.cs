/// <summary>
/// NowhereBattleSessionにおいて
/// 時間経過でアクションを起こす必要がある存在。
/// </summary>
public interface ITimerObject
{
    int dexterity{get;}
    void NoticeTime();
}