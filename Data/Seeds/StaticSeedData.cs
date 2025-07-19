using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using JapaneseLearningPlatform.Data.ViewModels;
using System.Collections.Generic;

namespace JapaneseLearningPlatform.Data.Seeds
{
        public static class StaticSeedData
        {
        public static IEnumerable<string> Roles => new[]
        {
            UserRoles.Admin,
            UserRoles.Partner,
            UserRoles.Learner
        };
        // 1. Videos
        public static IReadOnlyList<Video> Videos { get; } = new[]
            {
            new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=rGrBHiuPlT0&ab_channel=JapanSocietyNYC",
                            VideoDescription = "This introductory sample lesson covers eight basic greetings:\r\n\r\nGood Morning - Ohayou (casual) gozaimasu (formal)\r\nGood Afternoon - Konnichiwa\r\nGood Evening - Konbanwa\r\nGoodbye - Sayounara\r\nGoodnight - Oyasumi nasai\r\nThank You - Arigatou (casual) gozaimasu (formal)\r\nExcuse me, I'm sorry - Sumimasen\r\nHow do you do (nice to meet you) - Hajimemashite, dozo yoroshiku",
                        },
                        new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=bOUqVC4XkOY&ab_channel=JapanSocietyNYC",
                            VideoDescription = "In this lesson you will learn how to count numbers in Japanese from 1 to 100.\r\n\r\n1 = ichi\r\n2 = ni\r\n3 = san\r\n4 = yon / shi\r\n5 = go\r\n6 = roku\r\n7 = nana / shichi\r\n8 = hachi\r\n9 = kyuu\r\n10 = jyuu\r\n11 = jyuu-ichi\r\n12 = jyuu-ni\r\n......\r\n20 = ni-jyuu\r\n30 = san-jyuu\r\n40 = yon-jyuu\r\n......\r\n100 = hyaku"

                        },
                        new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=JnoZE51WZg4&t=1s&ab_channel=JapanSocietyNYC",
                            VideoDescription = "In this lesson you will learn both the days of the week and the days of the month in Japanese.\r\n\r\nDays of the Week:\r\n\r\nMonday = Getsuyoubi\r\nTuesday = Kayoubi\r\nWednesday = Suiyoubi\r\nThursday = Mokuyoubi\r\nFriday = Kinyoubi\r\nSaturday = Doyoubi\r\nSunday = Nichiyoubi"

                        },
                        new Video()
                        {   VideoURL = "https://www.youtube.com/watch?v=KUIWRsVZZZA&t=5s&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Ikimasu = to go\r\nIkimasen = (negative) to go\r\nIkimashita = (past) to go\r\nIkimasen deshita = (past negative) to go\r\ndepato = department store\r\nshigoto = work\r\npati = party\r\neki = station\r\nkyou = today\r\nkino = yesterday\r\nashita = tomorrow\r\nasatte = day after tomorrow\r\nototoi = day before yesterday\r\nhai = yes\r\niie = no"

                        },
                        new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=SzG9STImtk0&ab_channel=HarupakaJapanese",
                            VideoDescription = "nomimasu = to drink\r\ntabemasu = to eat\r\nmimasu = to see, to watch\r\nkikimasu = to hear, to listen\r\nnani = what\r\nashita = tomorrow\r\nkyou = today\r\nkino = yesterday\r\neiga = movie\r\nkohi = coffee\r\nsuteki = steak\r\nringo = apple\r\nrajio = radio\r\nnihon no ongaku = japanese music\r\nterebi = tv\r\nmiruku = milk\r\nwain = wine\r\nkupukekki = cupcake\r\nka = used at end of sentence to form question\r\no = particle used after object\r\nhai = yes\r\niie = no\r\n\r\nSentence structure:\r\n\r\ntime + object + o + verb\r\ntime + nani + o + verb + ka (to ask what someone is eating, drinking, listening to, watching)"

                        },
                        new Video() //new 6
                        {
                            VideoURL = "https://www.youtube.com/watch?v=SDpbnqdUs9Y&ab_channel=OKJapanese",
                            VideoDescription = "You can read and write the 5 Hiragana letters on K-line in 13 minutes! Let's enjoy the mnemonics and games to learn 'a, i, u, e, o'. 😊"
                        },
                        new Video() //new 7
                        {
                            VideoURL = "https://www.youtube.com/watch?v=WMtufcM4w14&ab_channel=OKJapanese",
                            VideoDescription = "You can read and write the 5 Hiragana letters on K-line in 13 minutes! Let's enjoy the mnemonics and games to learn 'ka, ki, ku, ke, ko'. 😊"
                        },
                        new Video() // new 8
                        {
                            VideoURL = "https://www.youtube.com/watch?v=ZLOIZGMe3eg&t=1s&ab_channel=OKJapanese",
                            VideoDescription = "Easiest way of learning Hiragana! You can read and write the 5 Hiragana letters on S-line in about 15 minutes! Let's enjoy the mnemonics and games to learn 'sa, shi, su, se, so'.😊"
                        },
                        new Video() // new 9
                        {
                            VideoURL = "https://www.youtube.com/watch?v=TZc5U3RpAb0&ab_channel=OKJapanese",
                            VideoDescription = "You can read and write the 5 Hiragana letters on T-line in 15 minutes! Let's enjoy the mnemonics and games to learn 'ta, chi, tsu, te, to'.😊"
                        },
                        new Video() // new 10
                        {
                            VideoURL = "https://www.youtube.com/watch?v=oOT0K2B3p98&ab_channel=OKJapanese",
                            VideoDescription = "You can read and write the 5 Hiragana letters on H-line in 15 minutes! Let's enjoy the mnemonics and games to learn 'ha, hi, hu(fu), he, ho'.😊"
                        },
                        new Video() // new 11
                        {
                            VideoURL = "https://www.youtube.com/watch?v=D8Kpv4MzV0I&ab_channel=OKJapanese",
                            VideoDescription = "Easiest way of learning Hiragana series! You can read and write the 5 Hiragana letters on M-line in 13 minutes! Let's enjoy the mnemonics and games to learn 'ma, mi, mu, me, mo'.😊",
                        },
                        new Video() // new 12
                        {
                            VideoURL = "https://www.youtube.com/watch?v=6gk1r9XJ3d4&ab_channel=OKJapanese",
                            VideoDescription = "You can read and write the 5 Hiragana letters on Y-line in 10 minutes! Let's enjoy the mnemonics and games to learn 'ya, yu, yo'.😊"
                        },
                        new Video() // new 13
                        {
                            VideoURL = "https://www.youtube.com/watch?v=R00ytkEZ7_4&ab_channel=OKJapanese",
                            VideoDescription = "Easiest and quickest way of learning Hiragana! You can read and write the 5 Hiragana letters on R-line in 15 minutes! Let's enjoy the mnemonics and games to learn 'ra, ri, ru, re, ro'.😊"
                        },
                        new Video() // new 14
                        {
                            VideoURL = "https://www.youtube.com/watch?v=QW_tz795DHI&ab_channel=OKJapanese",
                            VideoDescription = "You won't forget Hiragana letters once you watch this video! Let's enjoy the mnemonics and games to learn 'wa, (w)o, n'.😊"
                        },
                        new Video() // new 15
                        {
                            VideoURL = "https://www.youtube.com/watch?v=U2q5GsB0swQ&ab_channel=KanameNaito",
                            VideoDescription = "The term \"subject\" often confuses Japanese learners. One of the most important component of Japanese sentence structure is the topic. You mention the topic, then make a comment about it. But the problem is, the subject sometimes is the topic, but not always. In Japanese, people don't construct a sentence like in English, \"subject+verb+object\". It is very important to know that in Japanese the subject and the topic is different, you should erase the concept \"subject\" when you study Japanese. In the video, I will introduce to you the basic Japanese sentence structure so that you will know how to form natural Japanese sentences. They are VERY easy once you get the hang of it, there's nothing complicated about it."
                        },
                        new Video() // new 16 JapanSocietyNYC lesson 6
                        {
                            VideoURL = "https://www.youtube.com/watch?v=ZGGufccTLso&t=6s&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Vocabulary:\r\n\r\nshimasu - to do (something)\r\nbenkyou o shimasu - to study\r\nshigoto o shimasu - to work\r\nkaigi o shimasu - to have a meeting\r\nkaimono o shimasu - to shop\r\njogingu o shimasu - to jog\r\npati o shimasu - to party\r\ntenisu o shimasu - to play tennis\r\nkyou - today\r\nashita - tomorrow\r\nkino - yesterday\r\nasatte - day after tomorrow\r\nototoi - day before yesterday\r\nuchi - house\r\nkouen - park\r\ndepato - department store\r\nkaisha - company (office building)\r\ngakko - school\r\ntomodachi - friend\r\nsensei - teacher\r\nhisho - secretary\r\n\r\nSentence structure:\r\n\r\nTime (day of the week) + Place + de (particle for action at a place) + Person + to (particle for \"with whom\") + verb"
                        },
                        new Video() // new 17 JapanSocietyNYC lesson 7
                        {
                            VideoURL = "https://www.youtube.com/watch?v=W0n-ODPwtzA&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Japan Society currently offers 12 comprehensive levels of Japanese, as well as a variety of specialized courses and workshops including shodō (Japanese calligraphy). Courses take place year round with Fall, Spring and Summer semesters along with intensive and specialized courses throughout the year, so please stop by or visit our website for more information.\r\nIn this lesson you will learn 2 very useful Japanese verbs \"agemasu\" - to give and \"moraimasu\" - to receive. Proper sentence structure will also be covered, adding places, people and time to the sentence.\r\n\r\nVocabulary:\r\n\r\nagemasu - to give\r\nmoraimasu - to receive\r\nkyou - today\r\nashita - tomorrow\r\nkino - yesterday\r\nasatte - day after tomorrow\r\nototoi - day before yesterday\r\ntokei - watch\r\nkeki - cake\r\nhon - book\r\ntakai - expensive\r\noishii - delicious\r\nnihon no hon - japanese book\r\ntomodachi - friend\r\n\r\nSentence structure:\r\n\r\nSubject + wa (particle) + Time (day of the week) + Person + ni (particle) + adjective + noun (object) + o (particle for \"object\") + verb (agemasu / moraimasu)"
                        },
                        new Video() // new 18 JapanSocietyNYC lesson 8
                        {
                            VideoURL = "https://www.youtube.com/watch?v=p9PEIsOzJ5E&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Please view previous lessons for additional vocabulary and grammar.\r\n\r\nIn this lesson you will learn how to say the members of your family and another person's family. In Japanese there is a distinction between the two.\r\n\r\nVocabulary:\r\n\r\nYour Family\r\nsofu - grandfather\r\nsobo - grandmother\r\nchichi - father\r\nhaha - mother\r\nani - older brother\r\nane - older sister\r\notouto - younger brother\r\nimouto - younger sister\r\nmusuko - son\r\nmusume - daughter\r\nryoushin - parents\r\nkodomo - children\r\nkazoku - family\r\nshujin - husband\r\nkanai/tsuma - wife\r\n\r\nAnother's Family\r\nojiisan - grandfather\r\nobaasan - grandmother\r\notousan - father\r\nokaasan - mother\r\noniisan - older brother\r\noneesan - older sister\r\notoutosan - younger brother\r\nimoutosan - younger sister\r\nmusukosan - son\r\nmusumesan - daughter\r\ngoryoushin - parents\r\nkodomosan - children\r\ngokazoku - parents\r\ngoshujin - husband\r\nokusan - wife"
                        },
                        new Video() // new 19 JapanSocietyNYC lesson 9
                        {
                            VideoURL = "https://www.youtube.com/watch?v=Pc86Xg2MX-U&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Japan Society currently offers 12 comprehensive levels of Japanese, as well as a variety of specialized courses and workshops including shodō (Japanese calligraphy). Courses take place year round with Fall, Spring and Summer semesters along with intensive and specialized courses throughout the year, so please stop by or visit our website for more information.\r\nIn this lesson you will learn how to tell time in Japanese! Be sure to practice your Japanese numbers as well."
                        },new Video() // new 20 JapanSocietyNYC lesson 10
                        {
                            VideoURL = "https://www.youtube.com/watch?v=Lo5_5k7EPIM&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Summary:\r\n\r\nparticle は (wa) = topic marker\r\nex. Mary goes. = Mary-san は (wa) ikimasu.\r\n\r\nparticle に (ni) - place + ni = indicates direction/place\r\nex. Mary goes to Boston. = Mary-san は (wa) Boston に (ni) ikimasu.\r\n\r\nparticle に (ni) - specific time + ni = indicates specific moment in time\r\nex. Mary goes to Boston at 3 o'clock. = Mary-san は (wa) Boston に (ni) sanji に (ni) ikimasu.\r\n\r\nFor relative time (ex. tomorrow, next week, last year, next month, etc.) you do not use the particle ni. Only use ni for specific time.\r\n\r\nparticle と (to) - with someone + to = indicates with someone\r\nex. Mary goes to Boston with her mother. = Mary-san は (wa) Okaasan と (to) Boston に (ni) ikimasu.\r\n\r\nparticle で (de) - transportation + de = indicates mode of transportation\r\nex. Mary goes to Boston on a plane. = Mary-san は (wa) hikouki で (de) Boston に (ni) ikimasu.\r\n\r\nFor more information about Japanese language classes at the Japan Society, please visit our website! Also, be sure to check out our other language videos and be sure to subscribe if you enjoy!"
                        },
                        new Video() // new 21 JapanSocietyNYC lesson 11
                        {
                            VideoURL = "https://www.youtube.com/watch?v=hiLQLGDMOEA&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Summary:\r\n\r\nparticle は (wa) = topic marker\r\nex. Mary goes. = Mary-san は (wa) ikimasu.\r\n\r\nparticle に (ni) - place + ni = indicates direction/place\r\nex. Mary goes to Boston. = Mary-san は (wa) Boston に (ni) ikimasu.\r\n\r\nparticle に (ni) - specific time + ni = indicates specific moment in time\r\nex. Mary goes to Boston at 3 o'clock. = Mary-san は (wa) Boston に (ni) sanji に (ni) ikimasu.\r\n\r\nFor relative time (ex. tomorrow, next week, last year, next month, etc.) you do not use the particle ni. Only use ni for specific time.\r\n\r\nparticle と (to) - with someone + to = indicates with someone\r\nex. Mary goes to Boston with her mother. = Mary-san は (wa) Okaasan と (to) Boston に (ni) ikimasu.\r\n\r\nparticle で (de) - transportation + de = indicates mode of transportation\r\nex. Mary goes to Boston on a plane. = Mary-san は (wa) hikouki で (de) Boston に (ni) ikimasu.\r\n\r\nFor more information about Japanese language classes at the Japan Society, please visit our website! Also, be sure to check out our other language videos and be sure to subscribe if you enjoy!"
                        },
                        new Video() // new 22 JapanSocietyNYC lesson 12
                        {
                            VideoURL = "https://www.youtube.com/watch?v=LO9F6pSVDv0&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Summary:\r\n \r\nMary-san wa ashita tomodachi to kissaten de ko-hi- wo nomimasu.\r\nメアリーさんはあしたともだちときっさてんでコーヒーをのみます。\r\nTomorrow, Mary and a friend will drink coffee at a café.\r\n \r\nNano wo nomimasuka?/ なにをのみますか？/ What will you drink?\r\n \r\nDoko de nomimasuka?/ どこでのみますか？/ Where are you drinking?\r\n \r\nDare to nomimasuka?/ だれとのみますか？/ Who will you drink with?\r\n \r\nDare ga nomimasuka?/ だれがのみますか？/ Who is the one drinking?\r\n \r\nKinou/ きのう/ Yesterday\r\nKyou/ きょう/ Today\r\nAshita/ あした/ Tomorrow\r\nSenshuu/ せんしゅう/ Last week\r\nKonshuu/ こんしゅう/ This week\r\nRaishuu/ らいしゅう/ Next week\r\nSengetsu/ せんげつ/ Last month\r\nKongetsu/ こんげつ/ This month\r\nRaigetsu/ らいげつ/ Next month\r\nKyonen/ きょねん/ Last week\r\nKotoshi/ ことし/ This year\r\nRainen/ らいねん/ Next year\r\n \r\nItsu nomimasuka/ いつのみますか？/ When to drink?\r\n \r\nMary-san wa sanji ni Tanaka-san ni aimasu/ メアリーさんはさんじにたなかさんにあいます。/ Mary will meet Tanaka-san at 3 o'clock\r\n \r\nDare ni aimasuka/ だれにあいますか？/who are you meeting?\r\n \r\nDare ga aimasuka/ だれがあいますか？/ who will be meeting?\r\n \r\nNanji ni aimasuka/ なんじにあいますか？/what time will you be meeting?\r\n \r\nNannichi ni aimasuka/ なんにちにあいますか？/what day will you be meeting?\r\n \r\nNanyoubi ni aimasuka/ なにようびにあいますか？/what day of the week will you be meeting?\r\n \r\nNangatsu ni aimasuka/ なんがつにあいますか？/what month will you be meeting?\r\n \r\nNannen ni aimasuka/ なんねんにあいますか？/what year will you be meeting?\r\n \r\nSuzuki-san wa kinyoubi ni Mary-san to densha de Boston ni ikimasu/すずきさんはきんようびにメアリーさんとでんしゃでボストンにいきます。/ Suzuki-san, on Friday, is going to Boston with Mary on the train.\r\n \r\nDoko ni ikimasuka/ どこにいきますか？/ Where are you going?\r\n \r\nNande ikimasuka/ なんでいきますか？/ How are you going?\r\n \r\nDare to ikimasuka/ だれといきますか？/ Who are you going with?\r\n \r\nNanyoubi ni ikimasuka/ なんようびにいきますか？/ What day of the week are you going?\r\n \r\nDarega ikimasuka/ だれがいきますか？/Who is going?\r\n \r\nNihongo wo benkyou shiteimasuka/ にほんごをべんきょうしていますか？/ Are you studying Japanese?\r\nDoushite nihongo wo benkyou shiteimasuka/ どうしてにほんごをべんきょうしていますか？/ Why are you studying Japanese?\r\nNihon ni ikimasukara/ にほんにいきますから/ It's because I'm going to Japan.\r\nOkane wo choking shiteimasuka/ おかねをちょきんしていますか？/ Are you saving up money?\r\nDoushite okane wo choking shiteimasuka/ どうしておかねをちょきんしていますか？/ Why are you saving up money\r\nAtarashii kuruma wo kaimasukara/ あたらしいくるまをかいますから/because I'm buying a new car"
                        },
                        new Video() // new 23 JapanSocietyNYC lesson 13
                        {
                            VideoURL = "https://www.youtube.com/watch?v=rFRJes0ic8Y&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Ue / うえ / Above\r\nShita / した / Under \r\nMae / まえ / In front\r\nUshiro / うしろ / Behind\r\nNaka / なか / Inside\r\nTonari / となり / Beside\r\nYoko / よこ / Beside\r\nChikaku / ちかく/ Near\r\nHako / はこ / Box\r\nIsu / いす / Chair\r\nKaban / かばん / Bag\r\nBoushi / ぼうし / Hat \r\nHon / ほん / Book\r\nKeitai / けいたい / Cell phone\r\nTokei / とけい / Watch\r\n\r\nHako no naka / はこのなかに / Inside the box\r\n\r\nHako no ue ni pen ga arimasu / はこのうえにペンがあります。 / There is a pen on top of the box.\r\n\r\nIsu no ue ni kaban ga arimasu / いすのうえにかばんがあります。 / There is a bag on the chair.\r\n\r\nHon no shita ni keitai ga arimasu / ほんのしたにけいたいがあります。 / There is a cell phone beneath the book.\r\n\r\nIkkai / いっかい/ 1st floor\r\nNikai / にかい/ 2nd floor\r\nSangai / さんがい/ 3rd floor\r\nYonkai / よんかい/ 4th floor\r\nGokai / ごかい/ 5th floor\r\nRokkai / ろっかい/ 6th floor\r\nNanakai / ななかい/ 7th floor\r\nHakkai / はっかい/ 8th floor\r\nKyuukai / きゅうかい/ 9th floor\r\nJuukai / じゅっかい/ 10th floor\r\n\r\nArimasu - used for non-living things\r\nImasu - used for living things\r\n\r\nSangai ni gingkou ga arimasu / さんがいにぎんこうがあります。 / There is a bank on the 3rd floor.\r\n\r\nHakkai ni Japan Society ga arimasu / はっかいにジャパンソサエティがあります。/ Japan Society is on the 8th floor.\r\n\r\nNikai ni Mary-san ga imasu / にかいにメアリさんがいます。 / Mary is on the 2nd floor.\r\n\r\nKuruma no naka ni Mary-san ga imasu / くるまのなかにメアリさんがいます。 / Mary is in the car."
                        },
                        new Video() // new 24 JapanSocietyNYC lesson 14
                        {
                            VideoURL = "https://www.youtube.com/watch?v=M9BuT65uNIA&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Summary:\r\n\r\nJouzu / じょうず・上手 / to be good at\r\nSuki / すき・好き / to like \r\nWakarimasu / わかります・分かります / to understand\r\n\r\nBasic structure:\r\n\r\n(Subject) wa (noun) ga (jouzu/suki/wakarimasu) (desu - with jouzu and suki) conjugate verbs for past/present/negative tense\r\n\r\nたなかさんはスキーがじょうずです／Tanaka-san wa sukii ga jouzu desu\r\nMr. Tanaka is good at skiiing.\r\n\r\nたなかさんはスキーがすきです／Tanaka-san wa sukii ga suki desu\r\nMr. Tanaka likes skiiing.\r\n\r\nたなかさんはフランスごがわかります／Tanaka-san wa furansugo ga wakarimasu\r\nMr. Tanaka knows (understands) French.\r\n\r\nPresent/Negative/Past/Past Negative:\r\n\r\nじょうずです／jouzu desu - (subject) is good at\r\nじょうずではありません／jouzu dewa arimasen - (subject) is not good at\r\nじょうずでした／jouzu deshita - (subject) was good at\r\nじょうずではありませんでした／jouzu dewa arimasen deshita - (subject) was not good at\r\n\r\nすきです／suki desu - (subject) likes\r\nすきではありません／suki dewa arimasen - (subject) does not like\r\nすきでした／suki deshita - (subject) liked\r\nすきではありませんでした／suki dewa arimasen deshita - (subject) did not like\r\n\r\nわかります／wakarimasu - (subject) understands\r\nわかりません／wakarimasen - (subject) does not understand\r\nわかりました／wakarimashita - (subject) understood\r\nわかりませんでした／wakarimasen deshita - (subject) did not understand\r\n\r\nLevels/Degrees:\r\n\r\nあまり（じょうずではありません）／ amari (jouzu dewa arimasen) - not so good at\r\nだいすき／daisuki - love\r\nとても（すきです）／　totemo (suki desu) - likes very much\r\nあまり（すきではありません）／ amari (suki dewa arimasen) - not like very much\r\nきらい／kirai - hates (does not like, strongly)\r\nすこし（わかります）／sukoshi (wakarimasu) - knows a little\r\nあまり（わかりません）／does not know very much\r\nぜんぜん（わかりません）／does not know at all\r\n\r\nkirai - hate\r\nzenzen - not at all\r\namari - not very much\r\nsukoshi - a little\r\ntotemo - very much\r\ndaisuki - love\r\n\r\nOther:\r\n\r\nわかいとき／wakai toki - when (subject) was young"
                        },
                        new Video() // new 25 JapanSocietyNYC lesson 15
                        {
                            VideoURL = "https://www.youtube.com/watch?v=2fx37sug4Oo&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Summary:\r\n\r\nい-adjectives\r\n\r\nあつい / 暑い / Hot\r\n\r\nさむい / 寒い / Cold\r\n\r\nたかい / 高い / Expensive\r\n\r\nやすい / 安い / Inexpensive\r\n\r\nおおきい / 大きい / Big\r\n\r\nちさい / 小さい / Small\r\n\r\nあたらしい / 新しい / New\r\n\r\nふるい / 古い / Old\r\n\r\nおいしい / 美味しい / Tastes good\r\n\r\nまずい / 不味い / Does not taste good\r\n\r\nあついです。 / 暑いです。 / It is hot.\r\n\r\nあつくないです。/ 暑くないです。/ It is not hot.\r\n\r\nあつ / 暑かったです。 / It was hot.\r\n\r\nあつ / 暑くなかったです。 / It was not hot.\r\n\r\nな-adjectives\r\n\r\nべんりな / 便利な / Convenient\r\n\r\nきれいな / Beautiful, Clean\r\n\r\nにぎやかな / 賑やかな / Lively\r\n\r\nげんきな / 元気な / Healthy\r\n\r\nしんせつな / 親切な / Kind, Gentle\r\n\r\nゆうめいな / 有名な / Famous\r\n\r\nげんきです。 / 元気です。 / I am well.\r\n\r\nげんきではありません。 / 元気ではありません。 / I am not well.\r\n\r\nげんきでした。 / 元気でした。 / I was well.\r\n\r\nげんきではありませんでした。 / 元気ではありませんでした。 / I was not well.\r\n\r\nおおきいかばんです。 / 大きい鞄です。 / It is a big bag.\r\n\r\nあたらしいかばんです。 / 新しい鞄です。 / It is a new bag.\r\n\r\nたかいかばんです。 / 高い鞄です。 / It is an expensive bag.\r\n\r\nかばんはおおきいです。 / 鞄は大きいです。 / The bag is big.\r\n\r\nかばんはあたらしいです。 / 鞄は新しいです。 / The bag is new.\r\n\r\nかばんはたかいです。 / 鞄は高いです。 / The bag is expensive.\r\n\r\nニューヨークはにぎやかなまちです。 / ニューヨークは賑やかな町です。 / New York is a lively city.\r\n\r\nニューヨークはきれいなまちです。 / ニューヨークはきれいな町です。 / New York is a beautiful city.\r\n\r\nニューヨークはゆうめいなまちです。 / ニューヨークは有名な町です。 / New York is a famous city.\r\n\r\nニューヨークはにぎやかです。 / ニューヨークは賑やかです。 / New York is lively.\r\n\r\nニューヨークはきれいです。 / New York is beautiful.\r\n\r\nニューヨークはゆうめいです。 / ニューヨークは有名です。 / New York is famous."
                        },
                        new Video() // new 26 JapanSocietyNYC lesson 16
                        {
                            VideoURL = "https://www.youtube.com/watch?v=6ABKZ8e0nGc&ab_channel=JapanSocietyNYC",
                            VideoDescription = "\r\nNotes\r\n\r\nThe basic form:\r\nQuestion: change ます to ませんか.\r\nAnswer: change ます to ましょう.\r\n\r\nExamples:\r\n映画を見ます。見ませんか？／えいがをみます。みませんか？\r\nI will see a movie. Do you want to see?\r\n\r\n映画を見ませんか？／えいがをみませんか？\r\nDo you want to see a movie?\r\n\r\nはい、見ましょう。／はい、みましょう。\r\nYes, let's see it.\r\n\r\n\r\nコンサートに行きます。コンサートに行きませんか？／コンサートにいきます。コンサートにいきませんか？\r\nI will go to a concert.  Do you want to go to the concert?\r\n\r\nはい、行きましょう。／はい、いきましょう。\r\nYes, let's go.\r\n\r\n\r\n晩ご飯を食べます。晩ご飯を食べませんか？／ばんごはんをたべます。ばんごはんをたべませんか？\r\nI will eat dinner. Do you want to eat dinner?\r\n\r\nはい、食べましょう。／はい、たべましょう。\r\nYes, let's eat.\r\n\r\n\r\nドライブをします。ドライブをしませんか？\r\nI will go for a drive. Do you want to go for a drive?\r\n\r\nはい、しましょう。\r\nYes, let's go.\r\n\r\n\r\nSensei's secret pyramid of listener's choice:\r\nTier 1— ましょう／Let's...\r\nTier 2— ましょうか？／Shall we...\r\nTier 3— ませんか？／Would you...\r\n\r\nThe listener's freedom to decide whether to accept or to decline the invitation increases from the top tier to the bottom tier.\r\n\r\n\r\nExample:\r\nスキーに行きませんか？／スキーにいきませんか？\r\nDo you want to go skiing?\r\n\r\nはい、行きましょう。／はい、いきましょう。\r\nYes, let's go.\r\n\r\nどこに行きましょうか？／どこにいきましょうか？\r\nWhere shall we go?\r\n\r\nコロラドに行きませんか？／コロラドにいきませんか？\r\nDo you want to go to Colorado?\r\n\r\n[コロラドはどうですか？]\r\n[How about Colorado?]\r\n\r\nいいですね！\r\n[That] sounds good!\r\n\r\nいつ行きましょうか？／いついきましょうか？\r\nWhen shall we go?\r\n\r\n来週、行きませんか？／らいしゅう、いきませんか？\r\n[Do you] want to go next week?\r\n\r\n[来週はどうですか？／らいしゅうはどうですか？]\r\n[How about next week?]\r\n\r\nそうしましょう。\r\nLet's do it. / Let's do so."
                        },
                        new Video() // new 27 JapanSocietyNYC lesson 17
                        {
                            VideoURL = "https://www.youtube.com/watch?v=T3hC03n_qWU&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Regular 1 verbs:\r\n\r\nKaimasu／かいます - to buy\r\nMachimasu／まちます - to wait\r\nKaerimasu／かえります - to return\r\nShinimasu／しにます - to die\r\nAsobimasu／あそびます - to play\r\nNomimasu／のみます - to drink\r\nKikimasu／ききます - to listen\r\nOyogimasu／およぎます - to swim\r\nHanashimasu／はなします - to talk\r\n\r\nConjugation for regular 1 verbs -- the Te-form song:\r\n\r\n1. i chi ri -- tte　いちり — って\r\ne.g.  Kaimasu／かいます — Katte／かって\r\nMachimasu／まちます — Matte／まって\r\nKaerimasu／かえります — Kaette／かえって\r\n\r\n2. ni bi mi -- nde　にびみ — んで\r\ne.g.  Shinimasu／しにます — Shinde／しんで\r\nAsobimasu／あそびます — Asonde／あそんで\r\nNomimasu／のみます— Nonde／のんで\r\n\r\n3. ki -- ite　き — いて \r\ne.g.  Kikimasu／ききます — Kiite／きいて\r\n\r\n4. gi -- ide　ぎ  — いで\r\ne.g.  Oyogimasu／およぎます— Oyoide／およいで\r\n\r\n5. shi -- shite　し — して\r\ne.g.  Hanashimasu／はなします — Hanashite／はなして\r\n\r\n\r\nRegular 2 verbs:\r\n\r\nTabemasu／たべます - to eat\r\nMimasu／みます - to see\r\nAgemasu／あげます - to give\r\nAkemasu／あけます - to open\r\nShimemasu／しめます - to close\r\nTodokemasu／とどけます - to deliver\r\nTsukemasu／つけます - to switch on\r\n\r\nConjugation for regular 2 verbs -- change --masu to --te:\r\n\r\ne.g. Tabemasu／たべます — Tabete／たべて\r\nMimasu／みます — Mite／みて\r\nAgemasu／あげます — Agete／あげて\r\nAkemasu／あけます — Akete／あけて\r\nShimemasu／しめます — Shimete／しめて\r\nTodokemasu／とどけます — Todokete／とどけて\r\nTsukemasu／つけます — Tsukete／つけて\r\n\r\nIrregular verbs:\r\n\r\nKimasu／きます - to come\r\nShimasu／します - to do\r\n\r\nConjugation for irregular verbs:\r\n\r\nKimasu／きます — Kite／きて\r\nShimasu／します — Shite／して\r\n\r\n-------\r\n\r\nStay tuned for future lessons on the Te-form and more!\r\n\r\nSubscribe and visit Japan Society for the latest information!"
                        },
                        new Video() // new 28 JapanSocietyNYC lesson 18
                        {
                            VideoURL = "https://www.youtube.com/watch?v=ZUM7wPLXqWA&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Example 1:\r\n\r\nAshita yunion sukuea ni ikimasu／あしたユニオンスクエアにいきます。／[I] will go to Union Square tomorrow.\r\nKutsu o kaimasu／くつをかいます。／[I] buy shoes.\r\nAshita yunion sukuea ni itte, kutsu o kaimasu／あしたユニオンスクエアにいって、くつをかいます。／[I] will go to Union Square and buy shoes tomorrow.\r\nKinou yunion sukuea ni itte, kutsu o kaimashita／きのうユニオンスクエアにいって、くつをかいました。／[I] went to Union Square and bought shoes yesterday.\r\n\r\n*Note* te-form does not change for past or present tense\r\n\r\n\r\nExample 2:\r\n\r\nTomodachi ni aimasu／ともだちにあいます。／[I] meet friends.\r\nKoohii o nomimasu／コーヒーをのみます。／[I] drink coffee.\r\nTomodachi ni atte, koohii o nomimasu／ともだちにあって、コーヒーをのみます。／[I] meet friends and drink coffee.\r\nMearii-san wa raishuu ni atte, koohii o nomimasu／メアリーさんはらいしゅうともだちにあって、コーヒーをのみます。／Mary will meet friends and drink coffee next week.\r\nMearii-san wa raishuu ni atte, kissaten de koohii o nomimasu／メアリーさんはらいしゅうともだちにあって、きっさてんでコーヒーをのみます。／Mary will meet friends and drink coffee at a cafe next week.\r\n\r\nExample 3:\r\n\r\nBangohan o tabemasu／ばんごはんをたべます。／[I] have dinner.\r\nEiga o mimasu／えいがをみます。／[I] watch a movie.\r\nEiga o mite, bangohan o tabemasu／えいがをみて、ばんごはんをたべます。／[I] watch a movie and have dinner.\r\nMearii-san wa kinou eiga o mite, bangohan o tabemashita／メアリーさんはきのうえいがをみて、ばんごはんをたべます。／Mary watched a movie and had dinner yesterday.\r\nMearii-san wa kinou nihon no eiga o mite, oishii bangohan o tabemashita／メアリーさんはきのうにほんのえいがをみて、おいしいばんごはんをたべました。／Mary watched a Japanese movie and had a delicious dinner yesterday.\r\n\r\n-------\r\n\r\nStay tuned for future lessons on the Te-form and more!\r\n"
                        },
                        new Video() // new 29 JapanSocietyNYC lesson 19
                        {
                            VideoURL = "https://www.youtube.com/watch?v=NBumXxwBBMo&ab_channel=JapanSocietyNYC",
                            VideoDescription = "\r\nPlease Listen:\r\nききます　--　きいてください／kikimasu -- kiitekudasai\r\nto listen (regular I verb)\r\n\r\nこのCDをきいてください。／Kono CD o kiitekudasai.\r\nPlease listen to this CD.\r\n\r\n\r\nPlease Read:\r\nよみます　--　よんでください／yomimasu -- yondekudasai\r\nto read (regular I verb)\r\n\r\nこのにほんのほんをよんでください／Kono Nihon no hon o yondekudasai.\r\nPlease read this Japanese book.\r\n\r\n\r\nPlease Watch:\r\nみます　--　みてください／mimasu -- mitekudasai\r\nto see (regular II verb)\r\n\r\nこのおもしろいえいがをみてください／Kono omoshiroi eiga o mitekudasai.\r\nPlease watch this interesting movie.\r\n\r\n\r\nPlease Deliver:\r\nとどけます　--　とどけてください／todokemasu -- todoketekudasai\r\nto deliver (regular II verb)\r\n\r\nこのパッケージとどけてください／Kono pakkeeji o todoketekudasai.\r\nPlease deliver this package.\r\n\r\n\r\nPlease Come:\r\nきます　--　きてください／kimasu -- kitekudasai\r\nto come (irregular verb)\r\n\r\nあしたにじにきてください／Ashita niji ni kitekudasai.\r\nPlease come at two o'clock tomorrow."
                        },
                        new Video() // new 30 JapanSocietyNYC lesson 20
                        {
                            VideoURL = "https://www.youtube.com/watch?v=jhByyxdNaZU&ab_channel=JapanSocietyNYC",
                            VideoDescription = "Notes:\r\n\r\nMay I read:\r\nよみます　--　よんでもいいですか／yomimasu　-- yonde mo ii desu ka (regular I verb)\r\n\r\nこのにほんのほんをよんでもいいですか？／Kono nihon no hon o yonde mo ii desu ka?\r\nMay I read this Japanese book?\r\n\r\n\r\nMay I drink:\r\nのみます　--　のんでもいいですか／nomimasu -- nonde mo ii desu ka (regular I verb)\r\n\r\nこのフランスのワインをのんでもいいですか？／Kono furansu no wain o nonde mo ii desu ka?\r\nMay I drink this French wine?\r\n\r\n\r\nMay I eat:\r\nたべます　--　たべてもいいですか／tabemasu -- tabete mo ii desu ka (regular II verb)\r\n\r\nこのケーキをたべてもいいですか？／Kono keeki o tabete mo ii desu ka?\r\nMay I eat this cake?\r\n\r\n\r\nMay I watch:\r\nみます　--　みてもいいですか／mimasu -- mite mo ii desu ka (regular II verb)\r\n\r\nこのえいがをみてもいいですか？／Kono eiga o mite mo ii desu ka?\r\nMay I watch this movie?\r\n\r\n\r\nMay I sit:\r\nすわります　--　すわってもいいですか／suwarimasu -- suwatte mo ii desu ka (regular I verb)\r\n\r\nこのいすにすわってもいいですか？／Kono isu ni suwatte mo ii desu ka?\r\nMay I sit on this chair?\r\n\r\n-------\r\n\r\nStay tuned for future lessons on the Te-form and more!"
                        },
        };

            // 2. Courses
            public static IReadOnlyList<CourseSeed> Courses { get; } = new[]
            {
                new CourseSeed
{
    Name        = "Ultimate Japanese Bootcamp: Speak Like a Native + JLPT N5-N1",
    Description = "Master Japanese from JLPT N5 to N1, covering grammar, vocabulary, and conversation with native speakers.",
    Price       = 390000,
    ImageURL    = "https://img-c.udemycdn.com/course/240x135/3821776_0ffd_3.jpg",
    Discount    = 79,
    Category    = CourseCategory.Advanced,
    Sections    = new[]
    {
            "Introduction",
                    "The First Row (あ行 - A-Line)",
                    "The Second Row (か行 - Ka-Line)",
                    "The Third Row (さ行 - Sa-Line)",
                    "The Fourth Row (た行 - Ta-Line)",
                    "The Fifth Row (は行 - Na-Line)",
                    "The Sixth Row (ま行 - Ma-Line)",
                "The Seventh Row (や行 - Ya-Line)",
                "The Eighth Row (ら行 - Ra-Line)",
                "The Ninth Row (わ行とん行 - Ya-Line & N-Line)"
    },
    QuizTitle = "Beginner Quiz",
    Questions = new[]
    {
        new QuizQuestion {
            QuestionText = "What does 'こ' mean?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "hi", IsCorrect = false },
                new() { OptionText = "ko", IsCorrect = true },
                new() { OptionText = "na", IsCorrect = false },
                new() { OptionText = "re", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "How do you say 'yo' in Japanese?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "や", IsCorrect = false },
                new() { OptionText = "ゆ", IsCorrect = false },
                new() { OptionText = "よ", IsCorrect = true },
                new() { OptionText = "ん", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "Which one is the correct honorific prefix? (Choose all that apply)",
            QuestionType = QuestionType.MultipleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "おう", IsCorrect = true },
                new() { OptionText = "えい", IsCorrect = true },
                new() { OptionText = "おー", IsCorrect = false },
                new() { OptionText = "えー", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "What is 'さ'?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "sa", IsCorrect = true },
                new() { OptionText = "te", IsCorrect = false },
                new() { OptionText = "to", IsCorrect = false },
                new() { OptionText = "shi", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "What is 'しつ'?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "shiba", IsCorrect = false },
                new() { OptionText = "nigai", IsCorrect = false },
                new() { OptionText = "shite", IsCorrect = false },
                new() { OptionText = "shitsu", IsCorrect = true }
            }
        },
        new QuizQuestion {
            QuestionText = "What is “Good morning” in Japanese?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "さよなら", IsCorrect = false },
                new() { OptionText = "こんにちは", IsCorrect = false },
                new() { OptionText = "おはよう", IsCorrect = true },
                new() { OptionText = "また", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "How do you say “Yes” in Japanese?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "いいえ", IsCorrect = false },
                new() { OptionText = "はい", IsCorrect = true },
                new() { OptionText = "は", IsCorrect = false },
                new() { OptionText = "いい", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "What does 「いいえ」 mean?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "Yes", IsCorrect = false },
                new() { OptionText = "Please", IsCorrect = false },
                new() { OptionText = "No", IsCorrect = true },
                new() { OptionText = "Wait", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "What is “me” in Japanese?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "め", IsCorrect = true },
                new() { OptionText = "す", IsCorrect = false },
                new() { OptionText = "わか", IsCorrect = false },
                new() { OptionText = "し", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "What is “student” in Japanese?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "せんせい", IsCorrect = false },
                new() { OptionText = "ともだち", IsCorrect = false },
                new() { OptionText = "こうはい", IsCorrect = false },
                new() { OptionText = "がくせい", IsCorrect = true }
            }
        }
    },
    Contents = new List<CourseContentSeed>
    {
        new() {
            Title        = "Watch: Introduction Video",
            SectionIndex = 0,
            Type         = ContentType.Video,
            VideoId      = 15
        },
        new() {
            Title        = "Watch: a i u e o",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 6
        },
        new() {
            Title        = "Watch: ka ki ku ke ko",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 7
        },
        new() {
            Title        = "Watch: sa shi tsu se so",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 8
        },
        new() {
            Title        = "Watch: ta chi tsu te to",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 9
        },
        new() {
            Title        = "Watch: ha hi hu(fu) he ho",
            SectionIndex = 5,
            Type         = ContentType.Video,
            VideoId      = 10
        },
        new() {
            Title        = "Watch: ma mi mu me mo",
            SectionIndex = 6,
            Type         = ContentType.Video,
            VideoId      = 11
        },
        new() {
            Title        = "Watch: ya yu yo",
            SectionIndex = 7,
            Type         = ContentType.Video,
            VideoId      = 12
        },
        new() {
            Title        = "Watch: ra ri ru re ro",
            SectionIndex = 8,
            Type         = ContentType.Video,
            VideoId      = 13
        },
        new() {
            Title        = "Watch: wa wo n",
            SectionIndex = 9,
            Type         = ContentType.Video,
            VideoId      = 14
        },
        new() {
            Title        = "Practice: Basic Final Quiz",
            SectionIndex = 9,
            Type         = ContentType.Quiz,
            QuizIndex    = 0
        }
    }
},
// this shit is too long, so I will not write it all out, but you can use the same structure as above for other courses
// course 2 
            new CourseSeed
{
    Name        = "Basic Japanese Lesson Series",
    Description = "Master Basic Japanese from JLPT N5, covering grammar, vocabulary, and conversation with native speakers!",
    Price       = 320000,
    ImageURL    = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731338/japanese_lesson_1_epmipj.jpg",
    Discount    = 80,
    Category    = CourseCategory.Advanced,
    Sections    = new[]
    {
            "Introduction",
                    "Simple Vocabulary, Time, Going to a Destination, verbs",
                    "To do smth, To Give & To Receive, Family Members Vocabulary, Time O' Clock, Particles",
                    "Interrogatives and Counters, Interrogatives, Locations Vocabulary, To like/understand/good at, Adjectives",
                    "Invitations, て form conjugation, て - Sentence Connection, て Kudasai, て mo ii desu ka",
                    "Final Quiz",
    },
    QuizTitle = "Beginner Quiz",
    Questions = new[]
    {
                    new QuizQuestion {
                QuestionText = "What does 'いく' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = true },
                    new() { OptionText = "to eat", IsCorrect = false },
                    new() { OptionText = "to see", IsCorrect = false },
                    new() { OptionText = "to sleep", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'じかん' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "place", IsCorrect = false },
                    new() { OptionText = "time", IsCorrect = true },
                    new() { OptionText = "destination", IsCorrect = false },
                    new() { OptionText = "person", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'to eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = true },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "のむ", IsCorrect = false },
                    new() { OptionText = "みる", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle used for destination?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = true },
                    new() { OptionText = "で", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'する' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = false },
                    new() { OptionText = "to do", IsCorrect = true },
                    new() { OptionText = "to make", IsCorrect = false },
                    new() { OptionText = "to say", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is '9 o'clock' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "きゅうじ", IsCorrect = true },
                    new() { OptionText = "くじ", IsCorrect = true },
                    new() { OptionText = "ここのじ", IsCorrect = false },
                    new() { OptionText = "きじ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which word means 'mother'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "おかあさん", IsCorrect = true },
                    new() { OptionText = "おとうさん", IsCorrect = false },
                    new() { OptionText = "おねえさん", IsCorrect = false },
                    new() { OptionText = "いもうと", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle for the direct object?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = true },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "は", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'どこ' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "What", IsCorrect = false },
                    new() { OptionText = "When", IsCorrect = false },
                    new() { OptionText = "Where", IsCorrect = true },
                    new() { OptionText = "Why", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the meaning of 'すき'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "hate", IsCorrect = false },
                    new() { OptionText = "like", IsCorrect = true },
                    new() { OptionText = "dislike", IsCorrect = false },
                    new() { OptionText = "understand", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is 'library' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "としょかん", IsCorrect = true },
                    new() { OptionText = "がっこう", IsCorrect = false },
                    new() { OptionText = "うち", IsCorrect = false },
                    new() { OptionText = "えき", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which one is a counter for people?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "にん", IsCorrect = true },
                    new() { OptionText = "まい", IsCorrect = false },
                    new() { OptionText = "ほん", IsCorrect = false },
                    new() { OptionText = "こ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'Please eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = false },
                    new() { OptionText = "たべてください", IsCorrect = true },
                    new() { OptionText = "たべました", IsCorrect = false },
                    new() { OptionText = "たべます", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does '〜てもいいですか' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "You must do it", IsCorrect = false },
                    new() { OptionText = "You can do it", IsCorrect = true },
                    new() { OptionText = "You shouldn't do it", IsCorrect = false },
                    new() { OptionText = "Please don't", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which is the て-form of 'のむ'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "のみて", IsCorrect = false },
                    new() { OptionText = "のんで", IsCorrect = true },
                    new() { OptionText = "のんだ", IsCorrect = false },
                    new() { OptionText = "のむて", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How to say 'Let's go' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "いきましょう", IsCorrect = true },
                    new() { OptionText = "いこう", IsCorrect = false },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "いきたい", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the correct way to ask for permission: 'May I see?'",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "みてもいいですか", IsCorrect = true },
                    new() { OptionText = "みてください", IsCorrect = false },
                    new() { OptionText = "みましたか", IsCorrect = false },
                    new() { OptionText = "みませんか", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What particle is used with 'like' (すき)?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "が", IsCorrect = true },
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "で", IsCorrect = false }
                }
            },

    },
    Contents = new List<CourseContentSeed>
    {
        new() {
            Title        = "Watch: Introduction",
            SectionIndex = 0,
            Type         = ContentType.Video,
            VideoId      = 1
        },
        new() {
            Title        = "Watch: How to count in Japanese",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 2
        },
        new() {
            Title        = "Watch: Days of the Week and Days of the Month",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 3
        },
        new() {
            Title        = "Watch: Going to a Destination",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 4
        },
        new() {
            Title        = "Watch: Verbs (Nomimasu, Tabemasu, Mimasu, Kikimasu)",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 5
        },
        new() {
            Title        = "Watch: To do (verb)",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 16
        },
        new() {
            Title        = "Watch: To give & To Receive",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 17
        },
        new() {
            Title        = "Watch: Family Members Vocabulary",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 18
        },
        new() {
            Title        = "Watch: Telling Time",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 19
        },
        new() {
            Title        = "Watch: Particles",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 20
        },
        //
        new() {
            Title        = "Watch: Interrogatives and Counters",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 21
        },
        new() {
            Title        = "Watch: Interrogatives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 22
        },
        new() {
            Title        = "Watch: Locations Vocabulary",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 23
        },
        new() {
            Title        = "Watch: To like, To understand, To be good at",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 24
        },
        new() {
            Title        = "Watch: Adjectives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 25
        },
        //
        new() {
            Title        = "Watch: Invitations Grammar",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 26
        },
        new() {
            Title        = "Watch: Te form conjugation",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 27
        },
        new() {
            Title        = "Watch: Sentence Connection",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 28
        },
        new() {
            Title        = "Watch: Te Kudasai",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 29
        },
        new() {
            Title        = "Watch: Te mo ii desu ka",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 30
        },
        //
        new() {
            Title        = "Practical: Basic Final Quiz",
            SectionIndex = 5,
            Type         = ContentType.Quiz,
            QuizIndex    = 0
        }
    }
},
            // end of 2
            new CourseSeed
            {
                Name        = "Complete Japanese Course: Learn Japanese for Beginners",
                Description = "Learn Japanese FAST…",
                Price       = 230000,
                ImageURL    = "https://img-c.udemycdn.com/course/240x135/3821776_0ffd_3.jpg",
                Discount    = 80,
                Category    = CourseCategory.Basic,
                Sections    = new[]
                {
                    "Getting Started",
                    "Grammar Part 1",
                    "Entering the Real Conversation",
                    "Grammar Part 2",
                    "Meeting People",
                    "20 Most Useful Japanese Vocabularies"
                },
                QuizTitle  = "Japanese for Beginners Quiz",
                Questions  = new[]
                {
                    new QuizQuestion {
                        QuestionText = "What does 'こんにちは' mean?",
                        QuestionType = QuestionType.SingleChoice,
                        Options = new List<QuizOption> {
                            new() { OptionText="Goodbye", IsCorrect=false },
                            new() { OptionText="Hello",   IsCorrect=true  },
                            new() { OptionText="Thank you",IsCorrect=false },
                            new() { OptionText="Yes",     IsCorrect=false }
                        }
                    },
                    // … 14 câu nữa
                },
                Contents = new List<CourseContentSeed> {
                    new() {
                        Title        = "Watch: Advanced Greetings Video",
                        SectionIndex = 0,
                        Type         = ContentType.Video,
                        VideoId      = 2
                    },
                    new() {
                        Title        = "Take: Honorific Quiz",
                        SectionIndex = 1,
                        Type         = ContentType.Quiz,
                        QuizIndex    = 0
                    }
                }
            },
            new CourseSeed
{
    Name        = "Honorific Japanese Mastery",
    Description = "Master Japanese honorifics and advanced expressions used in formal settings.",
    Price       = 250000,
    ImageURL    = "https://i.pinimg.com/736x/98/3b/95/983b952e223ea927d0372dbb5144b8ea.jpg",
    Discount    = 40,
    Category    = CourseCategory.Advanced,

    Sections = new[]
    {
        "Understanding Honorifics",
        "Honorific Verbs in Action",
        "Final Quiz: Test Your Mastery"
    },

    QuizTitle = "Honorific Mastery Final Quiz",

    Questions = new[]
    {
        new QuizQuestion {
            QuestionText = "What is the polite form of 'する'?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "なさる", IsCorrect = true },
                new() { OptionText = "する", IsCorrect = false },
                new() { OptionText = "いらっしゃる", IsCorrect = false },
                new() { OptionText = "やる", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "Which of these is an honorific prefix?",
            QuestionType = QuestionType.MultipleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "お", IsCorrect = true },
                new() { OptionText = "ご", IsCorrect = true },
                new() { OptionText = "と", IsCorrect = false },
                new() { OptionText = "や", IsCorrect = false }
            }
        },
        new QuizQuestion {
            QuestionText = "How would you say 'teacher is coming' using honorifics?",
            QuestionType = QuestionType.SingleChoice,
            Options = new List<QuizOption> {
                new() { OptionText = "先生が来る", IsCorrect = false },
                new() { OptionText = "先生がいらっしゃる", IsCorrect = true },
                new() { OptionText = "先生がいる", IsCorrect = false },
                new() { OptionText = "先生がなさる", IsCorrect = false }
            }
        }
    },

    Contents = new List<CourseContentSeed>
    {
        new()
        {
            Title = "Watch: What Are Honorifics?",
            SectionIndex = 0,
            Type = ContentType.Video,
            VideoId = 5
        },
        new()
        {
            Title = "Watch: Honorific Verbs in Real Dialogues",
            SectionIndex = 1,
            Type = ContentType.Video,
            VideoId = 6
        },
        new()
        {
            Title = "Practice: Final Quiz",
            SectionIndex = 2,
            Type = ContentType.Quiz,
            QuizIndex = 0
        }
    }
},
            // course 4 
            new CourseSeed
{
    Name        = "Basic Japanese Lesson Bootcamp For Beginner",
    Description = "Master Basic Japanese, covering grammar, vocabulary, and conversation with native speakers!",
    Price       = 400000,
    ImageURL    = "https://scontent.fsgn2-6.fna.fbcdn.net/v/t39.30808-6/494596807_1066825622149060_515537649384938644_n.jpg?_nc_cat=111&ccb=1-7&_nc_sid=127cfc&_nc_eui2=AeGqAYUUnUEKMkS46tcVl-iXLTq7tP5iLF8tOru0_mIsX-9Gwv09u2cJRgu0niPnL2k9nCNVNCjw7HFI4IZG08fW&_nc_ohc=lYX-EhRXiSEQ7kNvwGZ8vNp&_nc_oc=AdnfBCMb4k_Iog8Xe9Cj7klzEZ7b6bitAcANeBCqVnXSRjnnmrPKpM6M2RVMZefTIKidvHCT1RuFHEdmRfKzHP4F&_nc_zt=23&_nc_ht=scontent.fsgn2-6.fna&_nc_gid=jCJkq0adI0DML6i48iCrsg&oh=00_AfRUyvB_xQhxCHEFvXG3JI2132vZLem1A4nRy_yZlNuVXA&oe=6874F21A",
    Discount    = 30,
    Category    = CourseCategory.Advanced,
    Sections    = new[]
    {
            "Introduction",
                    "Simple Vocabulary, Time, Going to a Destination, verbs",
                    "To do smth, To Give & To Receive, Family Members Vocabulary, Time O' Clock, Particles",
                    "Interrogatives and Counters, Interrogatives, Locations Vocabulary, To like/understand/good at, Adjectives",
                    "Invitations, て form conjugation, て - Sentence Connection, て Kudasai, て mo ii desu ka",
                    "Final Quiz",
    },
    QuizTitle = "Beginner Quiz",
    Questions = new[]
    {
                    new QuizQuestion {
                QuestionText = "What does 'いく' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = true },
                    new() { OptionText = "to eat", IsCorrect = false },
                    new() { OptionText = "to see", IsCorrect = false },
                    new() { OptionText = "to sleep", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'じかん' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "place", IsCorrect = false },
                    new() { OptionText = "time", IsCorrect = true },
                    new() { OptionText = "destination", IsCorrect = false },
                    new() { OptionText = "person", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'to eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = true },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "のむ", IsCorrect = false },
                    new() { OptionText = "みる", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle used for destination?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = true },
                    new() { OptionText = "で", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'する' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = false },
                    new() { OptionText = "to do", IsCorrect = true },
                    new() { OptionText = "to make", IsCorrect = false },
                    new() { OptionText = "to say", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is '9 o'clock' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "きゅうじ", IsCorrect = true },
                    new() { OptionText = "くじ", IsCorrect = true },
                    new() { OptionText = "ここのじ", IsCorrect = false },
                    new() { OptionText = "きじ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which word means 'mother'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "おかあさん", IsCorrect = true },
                    new() { OptionText = "おとうさん", IsCorrect = false },
                    new() { OptionText = "おねえさん", IsCorrect = false },
                    new() { OptionText = "いもうと", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle for the direct object?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = true },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "は", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'どこ' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "What", IsCorrect = false },
                    new() { OptionText = "When", IsCorrect = false },
                    new() { OptionText = "Where", IsCorrect = true },
                    new() { OptionText = "Why", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the meaning of 'すき'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "hate", IsCorrect = false },
                    new() { OptionText = "like", IsCorrect = true },
                    new() { OptionText = "dislike", IsCorrect = false },
                    new() { OptionText = "understand", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is 'library' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "としょかん", IsCorrect = true },
                    new() { OptionText = "がっこう", IsCorrect = false },
                    new() { OptionText = "うち", IsCorrect = false },
                    new() { OptionText = "えき", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which one is a counter for people?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "にん", IsCorrect = true },
                    new() { OptionText = "まい", IsCorrect = false },
                    new() { OptionText = "ほん", IsCorrect = false },
                    new() { OptionText = "こ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'Please eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = false },
                    new() { OptionText = "たべてください", IsCorrect = true },
                    new() { OptionText = "たべました", IsCorrect = false },
                    new() { OptionText = "たべます", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does '〜てもいいですか' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "You must do it", IsCorrect = false },
                    new() { OptionText = "You can do it", IsCorrect = true },
                    new() { OptionText = "You shouldn't do it", IsCorrect = false },
                    new() { OptionText = "Please don't", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which is the て-form of 'のむ'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "のみて", IsCorrect = false },
                    new() { OptionText = "のんで", IsCorrect = true },
                    new() { OptionText = "のんだ", IsCorrect = false },
                    new() { OptionText = "のむて", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How to say 'Let's go' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "いきましょう", IsCorrect = true },
                    new() { OptionText = "いこう", IsCorrect = false },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "いきたい", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the correct way to ask for permission: 'May I see?'",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "みてもいいですか", IsCorrect = true },
                    new() { OptionText = "みてください", IsCorrect = false },
                    new() { OptionText = "みましたか", IsCorrect = false },
                    new() { OptionText = "みませんか", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What particle is used with 'like' (すき)?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "が", IsCorrect = true },
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "で", IsCorrect = false }
                }
            },

    },
    Contents = new List<CourseContentSeed>
    {
        new() {
            Title        = "Watch: Introduction",
            SectionIndex = 0,
            Type         = ContentType.Video,
            VideoId      = 1
        },
        new() {
            Title        = "Watch: How to count in Japanese",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 2
        },
        new() {
            Title        = "Watch: Days of the Week and Days of the Month",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 3
        },
        new() {
            Title        = "Watch: Going to a Destination",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 4
        },
        new() {
            Title        = "Watch: Verbs (Nomimasu, Tabemasu, Mimasu, Kikimasu)",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 5
        },
        new() {
            Title        = "Watch: To do (verb)",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 16
        },
        new() {
            Title        = "Watch: To give & To Receive",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 17
        },
        new() {
            Title        = "Watch: Family Members Vocabulary",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 18
        },
        new() {
            Title        = "Watch: Telling Time",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 19
        },
        new() {
            Title        = "Watch: Particles",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 20
        },
        //
        new() {
            Title        = "Watch: Interrogatives and Counters",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 21
        },
        new() {
            Title        = "Watch: Interrogatives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 22
        },
        new() {
            Title        = "Watch: Locations Vocabulary",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 23
        },
        new() {
            Title        = "Watch: To like, To understand, To be good at",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 24
        },
        new() {
            Title        = "Watch: Adjectives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 25
        },
        //
        new() {
            Title        = "Watch: Invitations Grammar",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 26
        },
        new() {
            Title        = "Watch: Te form conjugation",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 27
        },
        new() {
            Title        = "Watch: Sentence Connection",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 28
        },
        new() {
            Title        = "Watch: Te Kudasai",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 29
        },
        new() {
            Title        = "Watch: Te mo ii desu ka",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 30
        },
        //
        new() {
            Title        = "Practical: Basic Final Quiz",
            SectionIndex = 5,
            Type         = ContentType.Quiz,
            QuizIndex    = 0
        }
    }
},
            // end of 4
            // course 5 
            new CourseSeed
{
    Name        = "Blue Japanese Lesson Series",
    Description = "Ez Basic Japanese from covering grammar, vocabulary, and conversation with native speakers!",
    Price       = 280000,
    ImageURL    = "https://scontent.fsgn2-9.fna.fbcdn.net/v/t39.30808-6/494605120_1066826015482354_111264291537643622_n.jpg?_nc_cat=106&ccb=1-7&_nc_sid=127cfc&_nc_eui2=AeHj-AnSICXCxzsdrm7eu20bhAzc12pT_haEDNzXalP-Fr0G7o9h6rPJUf5WO2OfPEgnKFrFiw8a8acsZ4viTdhj&_nc_ohc=Bm-T3dM_fNkQ7kNvwEhv5BQ&_nc_oc=AdlBCU_io1iU7FJaScmbRCHXTwv28ilQW83qxBfFyLQjoNjEwdQvPKGlcAgwVykJN49vLs_pr64nIRwC-k2DHNnH&_nc_zt=23&_nc_ht=scontent.fsgn2-9.fna&_nc_gid=QWSljFk_-elxjIOEg8Dn2A&oh=00_AfSislzsWsIm6hlU1Hab5QudxFTsKIt4w-KvAn0LmgLc2Q&oe=68750D27",
    Discount    = 76,
    Category    = CourseCategory.Advanced,
    Sections    = new[]
    {
            "Introduction",
                    "Simple Vocabulary, Time, Going to a Destination, verbs",
                    "To do smth, To Give & To Receive, Family Members Vocabulary, Time O' Clock, Particles",
                    "Interrogatives and Counters, Interrogatives, Locations Vocabulary, To like/understand/good at, Adjectives",
                    "Invitations, て form conjugation, て - Sentence Connection, て Kudasai, て mo ii desu ka",
                    "Final Quiz",
    },
    QuizTitle = "Beginner Quiz",
    Questions = new[]
    {
                    new QuizQuestion {
                QuestionText = "What does 'いく' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = true },
                    new() { OptionText = "to eat", IsCorrect = false },
                    new() { OptionText = "to see", IsCorrect = false },
                    new() { OptionText = "to sleep", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'じかん' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "place", IsCorrect = false },
                    new() { OptionText = "time", IsCorrect = true },
                    new() { OptionText = "destination", IsCorrect = false },
                    new() { OptionText = "person", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'to eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = true },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "のむ", IsCorrect = false },
                    new() { OptionText = "みる", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle used for destination?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = true },
                    new() { OptionText = "で", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'する' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = false },
                    new() { OptionText = "to do", IsCorrect = true },
                    new() { OptionText = "to make", IsCorrect = false },
                    new() { OptionText = "to say", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is '9 o'clock' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "きゅうじ", IsCorrect = true },
                    new() { OptionText = "くじ", IsCorrect = true },
                    new() { OptionText = "ここのじ", IsCorrect = false },
                    new() { OptionText = "きじ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which word means 'mother'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "おかあさん", IsCorrect = true },
                    new() { OptionText = "おとうさん", IsCorrect = false },
                    new() { OptionText = "おねえさん", IsCorrect = false },
                    new() { OptionText = "いもうと", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle for the direct object?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = true },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "は", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'どこ' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "What", IsCorrect = false },
                    new() { OptionText = "When", IsCorrect = false },
                    new() { OptionText = "Where", IsCorrect = true },
                    new() { OptionText = "Why", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the meaning of 'すき'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "hate", IsCorrect = false },
                    new() { OptionText = "like", IsCorrect = true },
                    new() { OptionText = "dislike", IsCorrect = false },
                    new() { OptionText = "understand", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is 'library' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "としょかん", IsCorrect = true },
                    new() { OptionText = "がっこう", IsCorrect = false },
                    new() { OptionText = "うち", IsCorrect = false },
                    new() { OptionText = "えき", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which one is a counter for people?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "にん", IsCorrect = true },
                    new() { OptionText = "まい", IsCorrect = false },
                    new() { OptionText = "ほん", IsCorrect = false },
                    new() { OptionText = "こ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'Please eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = false },
                    new() { OptionText = "たべてください", IsCorrect = true },
                    new() { OptionText = "たべました", IsCorrect = false },
                    new() { OptionText = "たべます", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does '〜てもいいですか' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "You must do it", IsCorrect = false },
                    new() { OptionText = "You can do it", IsCorrect = true },
                    new() { OptionText = "You shouldn't do it", IsCorrect = false },
                    new() { OptionText = "Please don't", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which is the て-form of 'のむ'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "のみて", IsCorrect = false },
                    new() { OptionText = "のんで", IsCorrect = true },
                    new() { OptionText = "のんだ", IsCorrect = false },
                    new() { OptionText = "のむて", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How to say 'Let's go' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "いきましょう", IsCorrect = true },
                    new() { OptionText = "いこう", IsCorrect = false },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "いきたい", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the correct way to ask for permission: 'May I see?'",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "みてもいいですか", IsCorrect = true },
                    new() { OptionText = "みてください", IsCorrect = false },
                    new() { OptionText = "みましたか", IsCorrect = false },
                    new() { OptionText = "みませんか", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What particle is used with 'like' (すき)?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "が", IsCorrect = true },
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "で", IsCorrect = false }
                }
            },

    },
    Contents = new List<CourseContentSeed>
    {
        new() {
            Title        = "Watch: Introduction",
            SectionIndex = 0,
            Type         = ContentType.Video,
            VideoId      = 1
        },
        new() {
            Title        = "Watch: How to count in Japanese",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 2
        },
        new() {
            Title        = "Watch: Days of the Week and Days of the Month",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 3
        },
        new() {
            Title        = "Watch: Going to a Destination",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 4
        },
        new() {
            Title        = "Watch: Verbs (Nomimasu, Tabemasu, Mimasu, Kikimasu)",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 5
        },
        new() {
            Title        = "Watch: To do (verb)",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 16
        },
        new() {
            Title        = "Watch: To give & To Receive",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 17
        },
        new() {
            Title        = "Watch: Family Members Vocabulary",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 18
        },
        new() {
            Title        = "Watch: Telling Time",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 19
        },
        new() {
            Title        = "Watch: Particles",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 20
        },
        //
        new() {
            Title        = "Watch: Interrogatives and Counters",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 21
        },
        new() {
            Title        = "Watch: Interrogatives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 22
        },
        new() {
            Title        = "Watch: Locations Vocabulary",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 23
        },
        new() {
            Title        = "Watch: To like, To understand, To be good at",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 24
        },
        new() {
            Title        = "Watch: Adjectives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 25
        },
        //
        new() {
            Title        = "Watch: Invitations Grammar",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 26
        },
        new() {
            Title        = "Watch: Te form conjugation",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 27
        },
        new() {
            Title        = "Watch: Sentence Connection",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 28
        },
        new() {
            Title        = "Watch: Te Kudasai",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 29
        },
        new() {
            Title        = "Watch: Te mo ii desu ka",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 30
        },
        //
        new() {
            Title        = "Practical: Basic Final Quiz",
            SectionIndex = 5,
            Type         = ContentType.Quiz,
            QuizIndex    = 0
        }
    }
},
            // end of 5
            // course 6
            new CourseSeed
{
    Name        = "Beauty Japanese Lesson Series",
    Description = "Master Basic Japanese from JLPT N5, covering grammar, vocabulary, and conversation with native speakers!",
    Price       = 220000,
    ImageURL    = "https://scontent.fsgn2-11.fna.fbcdn.net/v/t39.30808-6/494536627_1066826018815687_1077379566943298459_n.jpg?_nc_cat=105&ccb=1-7&_nc_sid=127cfc&_nc_eui2=AeFbrFRalj1c2Ai7LY0FAjWZqAkZt9LhataoCRm30uFq1uMbKWpyMISZM53qt75Vo10MxcTRB2akh-D_6Q2mRbLi&_nc_ohc=q1y__9VZqLUQ7kNvwEm6ayt&_nc_oc=AdlKu-lEJ5CM-4Pg2SDE6zMsiwj07Rjk1syp2gCx7-JoccDxpZ7O-v7hqfCA8qSaRNE8cjWUhpdQDug32CIzDrgV&_nc_zt=23&_nc_ht=scontent.fsgn2-11.fna&_nc_gid=0eoHrSZJSRRN5Oukqlqeow&oh=00_AfTCQnNlifhrKthq9NASx-DYwD2xNW0_6isAauaAL2LLxw&oe=6874F726",
    Discount    = 60,
    Category    = CourseCategory.Advanced,
    Sections    = new[]
    {
            "Introduction",
                    "Simple Vocabulary, Time, Going to a Destination, verbs",
                    "To do smth, To Give & To Receive, Family Members Vocabulary, Time O' Clock, Particles",
                    "Interrogatives and Counters, Interrogatives, Locations Vocabulary, To like/understand/good at, Adjectives",
                    "Invitations, て form conjugation, て - Sentence Connection, て Kudasai, て mo ii desu ka",
                    "Final Quiz",
    },
    QuizTitle = "Beginner Quiz",
    Questions = new[]
    {
                    new QuizQuestion {
                QuestionText = "What does 'いく' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = true },
                    new() { OptionText = "to eat", IsCorrect = false },
                    new() { OptionText = "to see", IsCorrect = false },
                    new() { OptionText = "to sleep", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'じかん' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "place", IsCorrect = false },
                    new() { OptionText = "time", IsCorrect = true },
                    new() { OptionText = "destination", IsCorrect = false },
                    new() { OptionText = "person", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'to eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = true },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "のむ", IsCorrect = false },
                    new() { OptionText = "みる", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle used for destination?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = true },
                    new() { OptionText = "で", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'する' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "to go", IsCorrect = false },
                    new() { OptionText = "to do", IsCorrect = true },
                    new() { OptionText = "to make", IsCorrect = false },
                    new() { OptionText = "to say", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is '9 o'clock' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "きゅうじ", IsCorrect = true },
                    new() { OptionText = "くじ", IsCorrect = true },
                    new() { OptionText = "ここのじ", IsCorrect = false },
                    new() { OptionText = "きじ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which word means 'mother'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "おかあさん", IsCorrect = true },
                    new() { OptionText = "おとうさん", IsCorrect = false },
                    new() { OptionText = "おねえさん", IsCorrect = false },
                    new() { OptionText = "いもうと", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the particle for the direct object?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "を", IsCorrect = true },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "は", IsCorrect = false },
                    new() { OptionText = "が", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does 'どこ' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "What", IsCorrect = false },
                    new() { OptionText = "When", IsCorrect = false },
                    new() { OptionText = "Where", IsCorrect = true },
                    new() { OptionText = "Why", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the meaning of 'すき'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "hate", IsCorrect = false },
                    new() { OptionText = "like", IsCorrect = true },
                    new() { OptionText = "dislike", IsCorrect = false },
                    new() { OptionText = "understand", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is 'library' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "としょかん", IsCorrect = true },
                    new() { OptionText = "がっこう", IsCorrect = false },
                    new() { OptionText = "うち", IsCorrect = false },
                    new() { OptionText = "えき", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which one is a counter for people?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "にん", IsCorrect = true },
                    new() { OptionText = "まい", IsCorrect = false },
                    new() { OptionText = "ほん", IsCorrect = false },
                    new() { OptionText = "こ", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How do you say 'Please eat' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "たべる", IsCorrect = false },
                    new() { OptionText = "たべてください", IsCorrect = true },
                    new() { OptionText = "たべました", IsCorrect = false },
                    new() { OptionText = "たべます", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What does '〜てもいいですか' mean?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "You must do it", IsCorrect = false },
                    new() { OptionText = "You can do it", IsCorrect = true },
                    new() { OptionText = "You shouldn't do it", IsCorrect = false },
                    new() { OptionText = "Please don't", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "Which is the て-form of 'のむ'?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "のみて", IsCorrect = false },
                    new() { OptionText = "のんで", IsCorrect = true },
                    new() { OptionText = "のんだ", IsCorrect = false },
                    new() { OptionText = "のむて", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "How to say 'Let's go' in Japanese?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "いきましょう", IsCorrect = true },
                    new() { OptionText = "いこう", IsCorrect = false },
                    new() { OptionText = "いく", IsCorrect = false },
                    new() { OptionText = "いきたい", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What is the correct way to ask for permission: 'May I see?'",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "みてもいいですか", IsCorrect = true },
                    new() { OptionText = "みてください", IsCorrect = false },
                    new() { OptionText = "みましたか", IsCorrect = false },
                    new() { OptionText = "みませんか", IsCorrect = false }
                }
            },
            new QuizQuestion {
                QuestionText = "What particle is used with 'like' (すき)?",
                QuestionType = QuestionType.SingleChoice,
                Options = new List<QuizOption> {
                    new() { OptionText = "が", IsCorrect = true },
                    new() { OptionText = "を", IsCorrect = false },
                    new() { OptionText = "に", IsCorrect = false },
                    new() { OptionText = "で", IsCorrect = false }
                }
            },

    },
    Contents = new List<CourseContentSeed>
    {
        new() {
            Title        = "Watch: Introduction",
            SectionIndex = 0,
            Type         = ContentType.Video,
            VideoId      = 1
        },
        new() {
            Title        = "Watch: How to count in Japanese",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 2
        },
        new() {
            Title        = "Watch: Days of the Week and Days of the Month",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 3
        },
        new() {
            Title        = "Watch: Going to a Destination",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 4
        },
        new() {
            Title        = "Watch: Verbs (Nomimasu, Tabemasu, Mimasu, Kikimasu)",
            SectionIndex = 1,
            Type         = ContentType.Video,
            VideoId      = 5
        },
        new() {
            Title        = "Watch: To do (verb)",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 16
        },
        new() {
            Title        = "Watch: To give & To Receive",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 17
        },
        new() {
            Title        = "Watch: Family Members Vocabulary",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 18
        },
        new() {
            Title        = "Watch: Telling Time",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 19
        },
        new() {
            Title        = "Watch: Particles",
            SectionIndex = 2,
            Type         = ContentType.Video,
            VideoId      = 20
        },
        //
        new() {
            Title        = "Watch: Interrogatives and Counters",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 21
        },
        new() {
            Title        = "Watch: Interrogatives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 22
        },
        new() {
            Title        = "Watch: Locations Vocabulary",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 23
        },
        new() {
            Title        = "Watch: To like, To understand, To be good at",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 24
        },
        new() {
            Title        = "Watch: Adjectives",
            SectionIndex = 3,
            Type         = ContentType.Video,
            VideoId      = 25
        },
        //
        new() {
            Title        = "Watch: Invitations Grammar",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 26
        },
        new() {
            Title        = "Watch: Te form conjugation",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 27
        },
        new() {
            Title        = "Watch: Sentence Connection",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 28
        },
        new() {
            Title        = "Watch: Te Kudasai",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 29
        },
        new() {
            Title        = "Watch: Te mo ii desu ka",
            SectionIndex = 4,
            Type         = ContentType.Video,
            VideoId      = 30
        },
        //
        new() {
            Title        = "Practical: Basic Final Quiz",
            SectionIndex = 5,
            Type         = ContentType.Quiz,
            QuizIndex    = 0
        }
    }
},
            // end of 6
            new CourseSeed
            {
                Name        = "Nihongo Lesson ep 7",
                Description = "Advanced Grammar and Expressions",
                Price       = 244000,
                ImageURL    = "https://i.pinimg.com/736x/5d/96/d1/5d96d1b0c27558870dfa02c5ffa54339.jpg",
                Discount    = 50,
                Category    = CourseCategory.Advanced,
                Sections    = new[] { "Advanced Greetings", "Honorific Quiz", "Situational Dialogues" },
                QuizTitle   = "Honorific Expressions Quiz",
                Questions   = Enumerable.Range(1,6).Select(i => new QuizQuestion {
                                  QuestionText = $"What is the correct usage of honorific in sentence {i}?",
                                  QuestionType = QuestionType.SingleChoice,
                                  Options      = new List<QuizOption> {
                                      new() { OptionText="Option 1", IsCorrect=true },
                                      new() { OptionText="Option 2", IsCorrect=false },
                                      new() { OptionText="Option 3", IsCorrect=false },
                                      new() { OptionText="Option 4", IsCorrect=false }
                                  }
                              }).ToArray(),
                Contents = new List<CourseContentSeed> {
                    new() { Title="Watch: Advanced Greetings Video", SectionIndex=0, Type=ContentType.Video, VideoId=2 },
                    new() { Title="Take: Honorific Quiz", SectionIndex=1, Type=ContentType.Quiz, QuizIndex=0 }
                }
            }
        };

            // 3. DailyWords
            public static IReadOnlyList<DailyWord> DailyWords { get; } = new[]
            {
        new DailyWord { JapaneseWord = "勉強", Romanji = "benkyou", Description = "Học tập, học hành (Study)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZtHBysJ4mTeiU03nAn9XJgr4_doYLAoudFA&s"},
        new DailyWord { JapaneseWord = "友達", Romanji = "tomodachi", Description = "Bạn bè (Friend)", ImageUrl = "https://media.istockphoto.com/id/541995888/photo/japanese-friends-group-selfie.jpg?s=612x612&w=0&k=20&c=easi1Getsu3m04cyY2vuGtznh1fTeiMJQ0AHauY7C70="},
        new DailyWord { JapaneseWord = "仕事", Romanji = "shigoto", Description = "Công việc (Career)", ImageUrl = "https://unlockjapan.jp/wp-content/uploads/2024/11/job-fair.png"},
        new DailyWord { JapaneseWord = "先生", Romanji = "sensei", Description = "Giáo viên (Teacher)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRhqIn41d5l8hQq57u8gav9dbcQFuRAQcxtlg&s"},
        new DailyWord { JapaneseWord = "時間", Romanji = "jikan", Description = "Thời gian (Time)", ImageUrl = "https://m.media-amazon.com/images/I/51wRuQHNHJL.jpg"},
        new DailyWord { JapaneseWord = "家族", Romanji = "kazoku", Description = "Gia đình (Family)", ImageUrl = "https://kated.com/wp-content/uploads/2020/03/JPN59a-Be-A-Guest-Of-A-Japanese-Family.jpg"},
        new DailyWord { JapaneseWord = "食べ物", Romanji = "tabemono", Description = "Thức ăn, món ăn (Food)", ImageUrl = "https://rimage.gnst.jp/livejapan.com/public/article/detail/a/00/02/a0002670/img/basic/a0002670_main.jpg"},
        new DailyWord { JapaneseWord = "飲み物", Romanji = "nomimono", Description = "Đồ uống (Drink)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ0-ReVo2VLbWu9tOAwKREJ4HNdhcWoz2sX5g&s"},
        new DailyWord { JapaneseWord = "学校", Romanji = "gakkou", Description = "Trường học (School)", ImageUrl = "https://web-japan.org/kidsweb/explore/calendar/assets/img/april/schoolyear01.jpg"},
        new DailyWord { JapaneseWord = "天気", Romanji = "tenki", Description = "Thời tiết (Weather)", ImageUrl = "https://rimage.gnst.jp/livejapan.com/public/article/detail/a/00/00/a0000213/img/basic/a0000213_main.jpg"},
        new DailyWord { JapaneseWord = "旅行", Romanji = "ryokou", Description = "Du lịch (Travel)", ImageUrl = "https://img.freepik.com/free-photo/woman-traveler-with-backpack-fushimi-inari-taisha-shrine-kyoto-japan_335224-88.jpg?semt=ais_hybrid&w=740"},
        new DailyWord { JapaneseWord = "音楽", Romanji = "ongaku", Description = "Âm nhạc (Music)", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/71/Japanese_music_icon.png/1200px-Japanese_music_icon.png"},
        new DailyWord { JapaneseWord = "本", Romanji = "hon", Description = "Sách (Book)", ImageUrl = "https://jtalkonline.com/wp-content/uploads/2020/11/Where-to-Get-Japanese-Novels-Outside-of-Japan-light-novels-scaled.jpg"},
        new DailyWord { JapaneseWord = "映画", Romanji = "eiga", Description = "Phim ảnh (Movie)", ImageUrl = "https://preview.redd.it/whats-your-favorite-film-from-japan-v0-elnpvoybgtad1.jpeg?width=1080&crop=smart&auto=webp&s=aa8a6dfc2aa7a118e02f59fa96db7396bd4e8f8e"},
        new DailyWord { JapaneseWord = "買い物", Romanji = "kaimono", Description = "Mua sắm (Shopping)", ImageUrl = "https://retailnext.net/_next/image?url=https%3A%2F%2Fimages.ctfassets.net%2Fuskqevaodrls%2F5JeL2yfgYL9POZSsLwdsnD%2Fee2260e2d93c439e30e80b8e0082cc94%2Fshutterstock_2294474927__2_.jpg&w=3840&q=75"},
        new DailyWord { JapaneseWord = "電話", Romanji = "denwa", Description = "Điện thoại (Mobile Phone)", ImageUrl = "https://www.japan-guide.com/g20/2223_02.jpg"},
        new DailyWord { JapaneseWord = "料理", Romanji = "ryouri", Description = "Nấu ăn, món ăn (Cook)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSzWjhP5PayWx3eRbHLf6YJxgh7cpXCT3_ozQ&s"},
        new DailyWord { JapaneseWord = "誕生日", Romanji = "tanjoubi", Description = "Sinh nhật (Birthday)", ImageUrl = "https://cdn.shopify.com/s/files/1/1083/2612/files/shutterstock_1240909954_480x480.jpg?v=1663330108"},
        new DailyWord { JapaneseWord = "動物", Romanji = "doubutsu", Description = "Động vật (Animal)", ImageUrl = "https://mlyhahhwafqm.i.optimole.com/cb:5ksX~4cf46/w:640/h:423/q:75/f:best/ig:avif/https://interacnetwork.com/the-content/cream/wp-content/uploads/2021/11/image8.jpg"},
        new DailyWord { JapaneseWord = "遊び", Romanji = "asobi", Description = "Chơi đùa, giải trí (Play, Entertainment)", ImageUrl = "https://d3c8ah58dul3sf.cloudfront.net/wp-content/uploads/2022/04/B5447956-8F73-4813-9A73-5BA0BF2DF186.jpeg"},
    };

        // 4. Users & Roles
        public static IReadOnlyList<UserSeed> Users = new List<UserSeed>
{
    new() { Email="lmp14589@gmail.com", FullName="Admin", Password="Phu123123@", Role="Admin" },
    new() { Email="giakhoiquang@gmail.com", FullName="Gia Khoi Partner", Password="Khoi2005.", Role="Partner", IsApproved = true },
    new() { Email="partner1@nihongo.com", FullName="Partner One", Password="Test123@", Role="Partner", IsApproved = true  },
    new() { Email="partner2@nihongo.com", FullName="Partner Two", Password="Test123@", Role="Partner", IsApproved = true },
    new() { Email="noobhoang@gmail.com", FullName="Hoang Nguyen", Password="Hoang2005.", Role="Learner" },
    new() { Email="hoang120305@gmail.com", FullName="Hoang Nguyen 2", Password="Hoang2005.", Role="Learner" },
    new() { Email="banneduser@gmail.com", FullName="Banned User", Password="Test123!@#", Role="Learner", IsBanned = true }
};


        // 5. Classroom templates
        public static IReadOnlyList<ClassroomTemplateSeed> ClassroomTemplates = new[]
            {
            new ClassroomTemplateSeed {
                Title="Beginner Japanese Conversation",
                Description="Focus on daily life dialogues",
                LanguageLevel=LanguageLevel.N5,
                PartnerEmail="giakhoiquang@gmail.com",
                ImageURL="https://…jpg"
            },
            new ClassroomTemplateSeed {
                Title="Intermediate Listening Practice",
                Description="Listen and discuss JLPT N4 audios",
                LanguageLevel=LanguageLevel.N4,
                PartnerEmail="giakhoiquang@gmail.com",
                ImageURL="https://…png"
            }
        };

            // 6. Classroom instances
            public static IReadOnlyList<ClassroomInstanceSeed> ClassroomInstances = new[]
            {
            new ClassroomInstanceSeed {
                TemplateIndex=0,
                StartOffsetDays=3,
                EndOffsetDays=33,
                ClassTime=new TimeSpan(19,0,0),
                MaxCapacity=10,
                Price=120000,
                IsPaid=true,
                GoogleMeetLink="https://meet.google.com/test-class1",
                Status=ClassroomStatus.Published
            },
            new ClassroomInstanceSeed {
                TemplateIndex=0,
                StartOffsetDays=-10,
                EndOffsetDays=20,
                ClassTime=new TimeSpan(20,0,0),
                MaxCapacity=8,
                Price=0,
                IsPaid=false,
                GoogleMeetLink="https://meet.google.com/free-class",
                Status=ClassroomStatus.InProgress
            }
        };

        // 7. Reports
        public static List<ReportViewModel> Reports { get; } = new()
        {
            new ReportViewModel {
                FullName    = "Nguyễn Văn A",
                Email       = "a.nguyen@example.com",
                Subject     = ReportSubject.Technical,
                OrderNumber = null,
                Message     = "Hệ thống báo lỗi 500 khi đăng nhập."
            },
            new ReportViewModel {
                FullName    = "Trần Thị B",
                Email       = "b.tran@example.com",
                Subject     = ReportSubject.Technical,
                OrderNumber = null,
                Message     = "Không thể reset mật khẩu."
            },

            new ReportViewModel {
                FullName    = "Lê Văn C",
                Email       = "c.le@example.com",
                Subject     = ReportSubject.Billing,
                OrderNumber = "ORD0001",
                Message     = "Hóa đơn không khớp số tiền."
            },
            new ReportViewModel {
                FullName    = "Phạm Thị D",
                Email       = "d.pham@example.com",
                Subject     = ReportSubject.Billing,
                OrderNumber = "ORD0002",
                Message     = "Thanh toán PayPal thất bại."
            },

            new ReportViewModel {
                FullName    = "Hoàng Văn E",
                Email       = "e.hoang@example.com",
                Subject     = ReportSubject.Courses,
                OrderNumber = null,
                Message     = "Muốn hỏi về lịch học tiếng Nhật cơ bản."
            },
            new ReportViewModel {
                FullName    = "Võ Thị F",
                Email       = "f.vo@example.com",
                Subject     = ReportSubject.Courses,
                OrderNumber = null,
                Message     = "Khóa N5 còn chỗ không?"
            },

            new ReportViewModel {
                FullName    = "Đặng Văn G",
                Email       = "g.dang@example.com",
                Subject     = ReportSubject.Partnerships,
                OrderNumber = null,
                Message     = "Muốn hợp tác tổ chức workshop."
            },
            new ReportViewModel {
                FullName    = "Ngô Thị H",
                Email       = "h.ngo@example.com",
                Subject     = ReportSubject.Partnerships,
                OrderNumber = null,
                Message     = "Yêu cầu báo giá khoá học doanh nghiệp."
            },

            new ReportViewModel {
                FullName    = "Trương Văn I",
                Email       = "i.truong@example.com",
                Subject     = ReportSubject.Feedback,
                OrderNumber = null,
                Message     = "Giao diện trang báo cáo chưa thân thiện."
            },
            new ReportViewModel {
                FullName    = "Bùi Thị K",
                Email       = "k.bui@example.com",
                Subject     = ReportSubject.Feedback,
                OrderNumber = null,
                Message     = "Có thể thêm tính năng chat bot hỗ trợ?"
            },

            new ReportViewModel {
                FullName    = "Mai Văn L",
                Email       = "l.mai@example.com",
                Subject     = ReportSubject.Other,
                OrderNumber = null,
                Message     = "Một số ý tưởng cải tiến khác…"
            },
            new ReportViewModel {
                FullName    = "Trần Thị M",
                Email       = "m.tran@example.com",
                Subject     = ReportSubject.Other,
                OrderNumber = null,
                Message     = "Cần hỗ trợ thêm về tài khoản giáo viên."
            },
            new ReportViewModel {
                FullName    = "Phan Minh Khôi",
                Email       = "minhkhoi@example.com",
                Subject     = ReportSubject.Technical,
                OrderNumber = null,
                Message     = "Mình gặp lỗi khi upload file profile."
            },
            new ReportViewModel {
                FullName    = "Trương Hữu Nam",
                Email       = "hunami@example.com",
                Subject     = ReportSubject.Technical,
                OrderNumber = null,
                Message     = "Không load được video bài giảng."
            },
            new ReportViewModel {
                FullName    = "Nguyễn Thị Lan",
                Email       = "lannguyen@example.com",
                Subject     = ReportSubject.Billing,
                OrderNumber = "BILL20250720",
                Message     = "Hóa đơn tự động gửi trễ."
            },
            new ReportViewModel {
                FullName    = "Lê Quang Đức",
                Email       = "quangduc@example.com",
                Subject     = ReportSubject.Billing,
                OrderNumber = "INV-4521",
                Message     = "Sai số tiền sau khi apply coupon."
            },
            new ReportViewModel {
                FullName    = "Võ Minh Anh",
                Email       = "minhanh@example.com",
                Subject     = ReportSubject.Courses,
                OrderNumber = null,
                Message     = "Muốn đăng ký thêm khóa trung cấp."
            },
            new ReportViewModel {
                FullName    = "Phạm Quốc Bảo",
                Email       = "quocbao@example.com",
                Subject     = ReportSubject.Courses,
                OrderNumber = null,
                Message     = "Khóa N4 có giảm giá không?"
            },
            new ReportViewModel {
                FullName    = "Đỗ Thị Hạnh",
                Email       = "hanhdo@example.com",
                Subject     = ReportSubject.Partnerships,
                OrderNumber = null,
                Message     = "Doanh nghiệp chúng tôi muốn hợp tác."
            },
            new ReportViewModel {
                FullName    = "Ngô Văn Tùng",
                Email       = "tungngo@example.com",
                Subject     = ReportSubject.Partnerships,
                OrderNumber = null,
                Message     = "Yêu cầu tư vấn gói đào tạo nội bộ."
            },
            new ReportViewModel {
                FullName    = "Bùi Thị Kim",
                Email       = "kim.bui@example.com",
                Subject     = ReportSubject.Feedback,
                OrderNumber = null,
                Message     = "Giao diện mobile chưa tối ưu."
            },
            new ReportViewModel {
                FullName    = "Lý Văn Sơn",
                Email       = "sonly@example.com",
                Subject     = ReportSubject.Feedback,
                OrderNumber = null,
                Message     = "Cần tùy chọn dark mode cho UI."
            },
            new ReportViewModel {
                FullName    = "Trần Thị Mai",
                Email       = "maitran@example.com",
                Subject     = ReportSubject.Other,
                OrderNumber = null,
                Message     = "Thông tin khác…"
            },
        };
    }
    }