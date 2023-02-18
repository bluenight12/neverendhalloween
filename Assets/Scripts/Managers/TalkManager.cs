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
        talkData.Add(100, new string[] { "10�� 31��, ������ �۷��� ���̴�.\n���ݱ��� �ݿ��� �ѵ��⸸ �߾��µ� ���� �� ģ���� ��Ƽ�� �ʴ��� ���.\n�� �Ű� ���� �ɱ�?\n������ ��.. �� ģ����� ģ���� ������\n�׷��� �۷��� ���̿� �ڽ�Ƭ ���� �� �԰� ���� �ϴ� �� �ƴѰ�..??\n��.. ��������"});

        //id = 1000 : ShopNPC
        talkData.Add(1000, new string[] {"��ȥ�� �ٲٷ� �Դ�?"});

        talkData.Add(2000, new string[] { "?" }); //���� 1
        talkData.Add(2001, new string[] { "?" }); //���� 2
        talkData.Add(2002, new string[] { "?" }); //���� 3
        talkData.Add(2003, new string[] { "?" }); //���� 4
        talkData.Add(2004, new string[] { "?" }); //���� 5
        talkData.Add(2005, new string[] { "?" }); //���� 6
        talkData.Add(2006, new string[] { "?" }); //���� 7
        talkData.Add(2007, new string[] { "?" }); //���� 8
        talkData.Add(2008, new string[] { "!" }); //���ΰ�
        talkData.Add(2009, new string[] { "!" }); //����
        talkData.Add(2010, new string[] { "!" }); //����
        talkData.Add(2011, new string[] { "���� �� ������...", "ä���� �� Ȥ�� �Ϻη�..." }); //����
        talkData.Add(2012, new string[] { "��... ���� �� ���ߴ���?", "�츮 �̹� ��Ƽ ������ '�Ǹ��� ���� �ĵ��� ���ɵ�' �̰ŵ�...", "�׷��� ���ΰ��� �츮�� �Ǹ� ���� �԰�,", "�ٸ� ģ������ �� ���� ���� �԰� ����� ���־��µ�..", "���� ������ �� �� �߾��� ����" }); //����
        talkData.Add(2013, new string[] { "...", "�׷�.. �� ���..?" }); //�ƿ�
        talkData.Add(2014, new string[] { "��... Ȥ�� ���� ���� �������� ���� �� �￩�����µ�", "�װ͵� �� ��������..." }); //����
        talkData.Add(2015, new string[] { "..." }); //�ƿ�
        talkData.Add(2016, new string[] { "..." }); //����
        talkData.Add(2017, new string[] { "��·�� ������ �� ���� �� �߸��� �����ϱ�", "�׳� �� �� �԰� ��Ƽ ���� �� �� ����.", "�츮 ���� ��հ� ����!" }); //����
        talkData.Add(2018, new string[] { "��..?", "��...", "�׷�..." }); //�ƿ�
        talkData.Add(2019, new string[] { "�� ���ȸ����� �ʳ�?" }); //���� 1
        talkData.Add(2020, new string[] { "����� �׳� ���� ����" }); //���� 2
        talkData.Add(2021, new string[] { "ä���̰� �̷� �� ����� �ִ� �ƴѵ�..", "�׸��� ���� �Ʊ� �� ���� �� â�� �� �ôµ�", "�����ʵ� ��û ���� �����־��ŵ�?" }); //���� 3
        talkData.Add(2022, new string[] { "..." }); //���� 4
        talkData.Add(2023, new string[] { "..." }); //���� 5
        talkData.Add(2024, new string[] { "����..." }); //���� 6
        talkData.Add(2025, new string[] { "���� ����..", "������ �����ֱ� �ȴ� �̰���" }); //���� 7
        talkData.Add(2026, new string[] { "Ǳ..." }); //���� 8
        talkData.Add(2027, new string[] { "�� �� ��", "������..����" }); //���� 7
        talkData.Add(2028, new string[] { "���� ��ü �� �߸�������.", "����� �ߴٰ� ���� �ͼ�?", "�ƴ� �׳�..", "���� �׷��ɱ�..?", "�̷��� �����ϰ� ���� �ٿ� �׳� ���� ���߰ڴ�.." }); //�ƿ�
        talkData.Add(2029, new string[] { "!!!!" });
        talkData.Add(2030, new string[] { "����.." });
        talkData.Add(2031, new string[] { "�� ��!! �峭ġ�� ��!" });
        talkData.Add(2032, new string[] { "���� ������ �ǰ�??" });
        talkData.Add(2033, new string[] { "��.. ��������..." });

        talkData.Add(3000, new string[] { "�ȳ�, ó�� ���� ģ��." }); //???
        talkData.Add(3001, new string[] { "!!!!!" }); //�ƿ�
        talkData.Add(3002, new string[] { "��Ѽ� �̾�������, �� �� Ǯ���ٷ�?" }); //???
        talkData.Add(3003, new string[] { "����, �����ϴ� �̰��� ó���� �� ������." }); //???
        talkData.Add(3004, new string[] { "�� ����ü ����...?" }); //�ƿ�
        talkData.Add(3005, new string[] { "�� A, �ͽ��̾�, �̰��� �ͽŵ��� ����.", "�ų� �ҷ���, ���డ �ΰ����� ��ƿ��� �־�", "�ʵ� �� �ΰ��� �� �� �� ����" }); //A
        talkData.Add(3006, new string[] { "���� ���ư��� �;�...", "���� ���ư����� ��� �ؾ� ��?" }); //�ƿ�
        talkData.Add(3007, new string[] { "�ٽ� ���ư��� ���ؼ��� ������ ������ ���� ��", "������ �� ��Ƹ������� �ͽŵ��� ������ �ž�", "��Ƹ����ٸ� �ʵ� ��ó�� �ͽ��� �ǰ���" }); //A
        talkData.Add(3008, new string[] { "��.. ������ ���ư� �� ������..?" }); //�ƿ�
        talkData.Add(3009, new string[] { "���� ������", "�׷� �� ���࿡�� ������ �� �ְ� �����ٰ�", "������ �ͽ��� ��ȥ�� ��������.", "�װ� ���� �� ��ȭ�����ٰ�" }); //A
        talkData.Add(3010, new string[] { "�ͽ��� ��ȥ�� ��� ��µ�?" }); //�ƿ�
        talkData.Add(3011, new string[] { "���� �ִ� �ͽŵ��� ��ġ�ϸ� ���� �� ���� �ž�", "�̰� �޾�" }); //A
        talkData.Add(3012, new string[] { "�� ������ �ִٸ� �ͽŵ��� ���� ����� �� �־�", "�տ� ������ ������ ���� �Ա��� �־�", "�׷� �̸�" }); //A

        talkData.Add(3100, new string[] { "������� �Ա���", "�ʵ� ���� �Ȱ��� �� ������..", "��·��, �̾��ϰ� �ƴ� ������" });

    }

    public string[] GetTalk(int id)
    {
        return talkData[id];
    }
}