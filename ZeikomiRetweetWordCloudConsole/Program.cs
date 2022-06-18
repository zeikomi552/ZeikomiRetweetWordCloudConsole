// See https://aka.ms/new-console-template for more information
using ZeikomiRetweetWordCloud.Models;
using ZeikomiRetweetWordCloud.Models.SQLite;



//コマンドライン引数を配列で取得する
string[] cmds = System.Environment.GetCommandLineArgs();


if (cmds.Length >= 4)
{
    Exec.Search(cmds[1], cmds[2]);
    Exec.CreateWordCloud(cmds[3], cmds[4]);
}
else
{
    string arg0 = string.Empty, arg1 = string.Empty, arg2 = string.Empty, arg3 = string.Empty, arg4 = string.Empty;

    if (cmds.Length > 0) arg0 = cmds[0];
    if (cmds.Length > 1) arg1 = cmds[1];
    if (cmds.Length > 2) arg2 = cmds[2];
    if (cmds.Length > 3) arg3 = cmds[3];
    if (cmds.Length > 4) arg4 = cmds[4];

    // 引数不足時のメッセージ
    Console.Write($"引数が不足しています。\r\n検索キーワード：{arg1}\r\nSQLiteファイルパス:{arg2}\r\nフォントディレクトリパス:{arg3}\r\nワードクラウド出力先フォルダ:{arg4}");
}

public class Exec
{
    #region 取得しているツイートId最大値の取得処理
    /// <summary>
    /// 取得しているツイートId最大値の取得処理
    /// </summary>
    /// <returns>ツイートId最大値</returns>
    public static string GetMaxId()
    {
        if (File.Exists(SQLiteDataContext.SQLitePath))
        {
            // 最新のツイートIDを取得
            var tmp = target_tweetBase.Select()?.Max(x => x.id);
            // 起動時に最新のIDをチェック
            return string.IsNullOrEmpty(tmp) ? "0" : tmp;
        }
        else
        {
            return "0";
        }
    }
    #endregion

    #region 検索処理
    /// <summary>
    /// キーワード検索処理
    /// #ワードクラウドでよろしく
    /// のキーワードをツイッター上から探す
    /// </summary>
    /// <param name="search_keyword">検索キーワード ex.#ワードクラウドでよろしく</param>
    /// <param name="sqlitepath">SQLiteファイルパス</param>
    /// <param name="font_dir">フォント保管先ディレクトリ</param>
    public static void Search(string search_keyword, string sqlitepath)
    {
        try
        {
            SQLiteDataContext.SQLitePath = sqlitepath;                                  // SQLiteパスのセット
            string since_id = GetMaxId();                                               // 最大IDの取得
            WordCloudManagerM.Search(search_keyword, since_id, sqlitepath);             // キーワードの検索
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    #endregion



    #region WordCloudの作成処理

    /// <summary>
    /// ワードクラウド作成処理
    /// </summary>
    /// <param name="font_dir">フォント保存ディレクトリ</param>
    /// <param name="out_dir">出力先ディレクトリ</param>
    public static void CreateWordCloud(string font_dir, string out_dir)
    {
        try
        {
            // WordCloudを作成していないリストを取得
            var list = target_tweetBase.Select().Where(x => x.wordcloud_status == 0).ToList();
            string work_dir = out_dir;

            // リストの分WordCloudを作成する
            foreach (var item in list)
            {
                string font_path = string.Empty;
                string color_map = string.Empty;
                // ツイートID指定してWordCloudを作成する
                string? msg = WordCloudManagerM.CreateWordcloud(work_dir, "all", item.id, SQLiteDataContext.SQLitePath, font_dir, 1, out font_path, out color_map);

                string search_key = string.Empty;
                if (!string.IsNullOrEmpty(msg))
                {
                    char gs = (char)29;

                    var msg_list = msg?.Split(gs);

                    foreach (var wordcloud_msg in msg_list!)
                    {
                        if (wordcloud_msg.Contains("search_keyword"))
                        {
                            search_key = wordcloud_msg.Replace("search_keyword=", "");
                            break;
                        }
                    }
                }

                try
                {
                    if (search_key.Equals("ゴメンナサイ"))
                    {
                        // 引用ツイート
                        QuotedTweetM.QuotedTweet(item.username, item.id, "ゴメンナサイ。うまく名詞を見つけられませんでした。\r\nキーワードを変えるかダブルクォートで囲んで頂けると・・・", Path.Combine(work_dir, item.id + ".png"));
                    }
                    else
                    {
                        // 引用ツイート
                        QuotedTweetM.QuotedTweet(item.username, item.id, "ワードクラウドで作成しました。対象キーワード[" + search_key + "]", Path.Combine(work_dir, item.id + ".png"));
                    }
                    var chg_value = new target_tweetBase();
                    chg_value.Copy(item);
                    chg_value.wordcloud_status = 1;
                    chg_value.colormap = color_map;
                    chg_value.font = System.IO.Path.GetFileNameWithoutExtension(font_path);
                    // データの更新
                    target_tweetBase.Update(item, chg_value);
                }
                catch
                {
                    QuotedTweetM.QuotedTweetNoMedia(item.username, item.id, "ごめんなさい。何等かの理由で失敗したみたいです。\r\n調査データに使わせてもらいます。ありがとうございます。");
                    var chg_value = new target_tweetBase();
                    chg_value.Copy(item);
                    chg_value.wordcloud_status = -1;
                    // データの更新
                    target_tweetBase.Update(item, chg_value);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    #endregion
}

