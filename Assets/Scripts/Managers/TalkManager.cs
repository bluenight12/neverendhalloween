using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager
{
    Dictionary<int, string[]> talkData;
    public bool isTalking = false;
    public void Init()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(100, new string[] { "10월 31일, 오늘은 핼러윈 데이다.\n지금까지 반에서 겉돌기만 했었는데 같은 반 친구가 파티에 초대해 줬다.\n날 신경 써준 걸까?\n오늘은 꼭.. 반 친구들과 친해져 봐야지\n그런데 핼러윈 데이엔 코스튬 같은 거 입고 가야 하는 거 아닌가..??\n뭐.. 괜찮겠지"});

        //id = 1000 : ShopNPC
        talkData.Add(1000, new string[] {"영혼을 바꾸러 왔니?"});

        talkData.Add(2000, new string[] { "?" }); //유령 1
        talkData.Add(2001, new string[] { "?" }); //유령 2
        talkData.Add(2002, new string[] { "?" }); //유령 3
        talkData.Add(2003, new string[] { "?" }); //유령 4
        talkData.Add(2004, new string[] { "?" }); //유령 5
        talkData.Add(2005, new string[] { "?" }); //유령 6
        talkData.Add(2006, new string[] { "?" }); //유령 7
        talkData.Add(2007, new string[] { "?" }); //유령 8
        talkData.Add(2008, new string[] { "!" }); //주인공
        talkData.Add(2009, new string[] { "!" }); //여자
        talkData.Add(2010, new string[] { "!" }); //남자
        talkData.Add(2011, new string[] { "뭐야 저 차림은...", "채린이 너 혹시 일부러..." }); //남자
        talkData.Add(2012, new string[] { "아... 내가 말 안했던가?", "우리 이번 파티 컨셉이 '악마의 집에 쳐들어온 유령들' 이거든...", "그래서 주인공인 우리는 악마 옷을 입고,", "다른 친구들은 다 유령 옷을 입고 오기로 돼있었는데..", "내가 너한텐 말 안 했었나 보네" }); //여자
        talkData.Add(2013, new string[] { "...", "그럼.. 난 어떡해..?" }); //아연
        talkData.Add(2014, new string[] { "음... 혹시 몰라서 내가 여벌옷을 여러 개 쟁여놨었는데", "그것도 다 떨어져서..." }); //여자
        talkData.Add(2015, new string[] { "..." }); //아연
        talkData.Add(2016, new string[] { "..." }); //여자
        talkData.Add(2017, new string[] { "어쨌든 너한테 말 안한 내 잘못도 있으니까", "그냥 그 옷 입고 파티 즐기면 될 것 같아.", "우리 오늘 재밌게 놀자!" }); //여자
        talkData.Add(2018, new string[] { "어..?", "응...", "그래..." }); //아연
        talkData.Add(2019, new string[] { "쟨 쪽팔리지도 않나?" }); //유령 1
        talkData.Add(2020, new string[] { "나라면 그냥 집에 갔다" }); //유령 2
        talkData.Add(2021, new string[] { "채린이가 이런 거 까먹을 애는 아닌데..", "그리고 내가 아까 옷 빌릴 때 창고 쓱 봤는데", "여벌옷도 엄청 많이 남아있었거든?" }); //유령 3
        talkData.Add(2022, new string[] { "..." }); //유령 4
        talkData.Add(2023, new string[] { "..." }); //유령 5
        talkData.Add(2024, new string[] { "뭐지..." }); //유령 6
        talkData.Add(2025, new string[] { "뭐긴 뭐야..", "쟤한텐 빌려주기 싫다 이거지" }); //유령 7
        talkData.Add(2026, new string[] { "풉..." }); //유령 8
        talkData.Add(2027, new string[] { "야 쉿 쉿", "들을라..ㅋㅋ" }); //유령 7
        talkData.Add(2028, new string[] { "내가 대체 뭘 잘못했을까.", "오라고 했다고 정말 와서?", "아님 그냥..", "나라서 그런걸까..?", "이렇게 비참하게 있을 바에 그냥 집에 가야겠다.." }); //아연
        talkData.Add(2029, new string[] { "!!!!" });
        talkData.Add(2030, new string[] { "뭐야.." });
        talkData.Add(2031, new string[] { "불 켜!! 장난치지 마!" });
        talkData.Add(2032, new string[] { "뭐야 정전인 건가??" });
        talkData.Add(2033, new string[] { "아.. 어지러워..." });

        talkData.Add(3000, new string[] { "안녕, 처음 보는 친구." }); //???
        talkData.Add(3001, new string[] { "!!!!!" }); //아연
        talkData.Add(3002, new string[] { "놀래켜서 미안하지만, 나 좀 풀어줄래?" }); //???
        talkData.Add(3003, new string[] { "고마워, 보아하니 이곳은 처음인 것 같은데." }); //???
        talkData.Add(3004, new string[] { "넌 도대체 뭐야...?" }); //아연
        talkData.Add(3005, new string[] { "난 A, 귀신이야, 이곳은 귀신들의 세계.", "매년 할로윈, 마녀가 인간들을 잡아오고 있어", "너도 그 인간들 중 한 명 같네" }); //A
        talkData.Add(3006, new string[] { "집에 돌아가고 싶어...", "집에 돌아가려면 어떻게 해야 해?" }); //아연
        talkData.Add(3007, new string[] { "다시 돌아가기 위해서는 마녀의 성으로 들어가야 해", "성에는 널 잡아먹으려는 귀신들이 가득할 거야", "잡아먹힌다면 너도 나처럼 귀신이 되겠지" }); //A
        talkData.Add(3008, new string[] { "나.. 집으로 돌아갈 수 있을까..?" }); //아연
        talkData.Add(3009, new string[] { "나를 도와줘", "그럼 널 마녀에게 도달할 수 있게 도와줄게", "나에게 귀신의 영혼을 가져와줘.", "그걸 통해 널 강화시켜줄게" }); //A
        talkData.Add(3010, new string[] { "귀신의 영혼은 어떻게 얻는데?" }); //아연
        talkData.Add(3011, new string[] { "성에 있는 귀신들을 퇴치하면 얻을 수 있을 거야", "이걸 받아" }); //A
        talkData.Add(3012, new string[] { "이 반지가 있다면 귀신들의 힘을 사용할 수 있어", "앞에 마녀의 성으로 가는 입구가 있어", "그럼 이만" }); //A

        talkData.Add(3100, new string[] { "여기까지 왔구나", "너도 나와 똑같이 될 거지만..", "어쨌든, 미안하게 됐다 꼬마야" });

    }

    public string[] GetTalk(int id)
    {
        return talkData[id];
    }
}