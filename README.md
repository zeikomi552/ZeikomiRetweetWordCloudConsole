# ZeikomiRetweetWordCloudConsole

![GitHub tag (latest SemVer)](https://img.shields.io/github/v/tag/zeikomi552/ZeikomiRetweetWordCloudConsole)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/zeikomi552/ZeikomiRetweetWordCloudConsole)
![GitHub](https://img.shields.io/github/license/zeikomi552/ZeikomiRetweetWordCloudConsole)

[![Twapi](https://github-readme-stats.vercel.app/api?username=zeikomi552)](https://github.com/zeikomi552/ZeikomiRetweetWordCloudConsole)

## アプリケーションの目的

本アプリケーションは特定のハッシュタグを持ったツイート内容を探し
ツイート内容をワードクラウド化しリツイートすることを目的としています。

## 機能概要

本アプリケーションの有する機能は以下通り。

1. Twitter API v2によるツイート内容検索(#キーワードを探す)
2. PythonライブラリJanomeによる形態素解析と名詞抽出
3. Twitter API v2による2.で抽出した名詞を用いたツイート内容のキーワード検索
4. PythonライブラリJanomeによる形態素解析と名詞抽出
5. Pythonライブラリwordcloudによるワードクラウド化
6. C# CoreTweetを用いたTwitter API v1.1によるワードクラウド画像付きリツイート


## 使用ライブラリ

### C#

- CoreTweet
- ClosedXML
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Tools

### Python

- sqlite3
- janome_tokenizer
- wordcloud
- demoji
- pandas


## 使用方法

1. 下準備
本アプリケーションは
zeikomi552/ZeikomiRetweetWordCloud
のiniファイルを使用します。
まずこちらでiniファイルの作成を行います。

2. 引数を指定してコマンド実行

ZeikomiRetweetWordCloudConsole.exe "ハッシュタグキーワード" "保存先SQLiteファイルパス" "フォント保管場所ディレクトリパス" "ワードクラウド出力先ディレクトリパス"

ex.

'''
ZeikomiRetweetWordCloudConsole.exe "#ワードクラウドでよろしく" "C:\ZeikomiRetweetWordCloudConsole\tmp.db" "C:\ZeikomiRetweetWordCloudConsole\font" "C:\ZeikomiRetweetWordCloudConsole\tmp"
'''

※フォントファイルは.ttfのみ使用することができます。