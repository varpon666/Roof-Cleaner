/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameHandler_ChatBubble : MonoBehaviour {

    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private Transform[] npcTransformArray = null;

    private int npcIndex;

    private void Start() {
        ChatBubble.Create(playerTransform, new Vector3(3, 8), ChatBubble.IconType.Neutral, "Here is some text!");

        FunctionPeriodic.Create(() => {
            Transform npcTransform = npcTransformArray[npcIndex];
            npcIndex = (npcIndex + 1) % npcTransformArray.Length;
            string message = GetRandomMessage();

            ChatBubble.IconType[] iconArray = 
                new ChatBubble.IconType[] { ChatBubble.IconType.Happy, ChatBubble.IconType.Neutral, ChatBubble.IconType.Angry };
            ChatBubble.IconType icon = iconArray[Random.Range(0, iconArray.Length)];

            ChatBubble.Create(npcTransform, new Vector3(3, 8), icon, message);
            
        }, 1.5f);
    }

    private string GetRandomMessage() {
        string[] messageArray = new string[] { 
            "Hello World!",
            "Good morning!",
            "Subscribe to Code Monkey!",
            "Check out Code Monkey on Steam!",
            "This is a really excellent place!",
            "I'm having so much fun walking around!",
            "I'm really sad about something",
            "I heard someone said something!",
            "I was wondering why the ball was getting bigger, then it hit me",
            "Did you hear about the guy whose whole left side was cut off? He’s all right now",
            "I'm reading a book about anti-gravity. It's impossible to put down!",
            "Don't trust atoms. They make up everything!",
            "What did the pirate say on his 80th birthday? AYE MATEY",
            "What’s Forrest Gump’s password? 1forrest1",
            "Two guys walk into a bar, the third one ducks.",
            "How many tickles does it take to make an octopus laugh? Ten-tickles",
            "Our wedding was so beautiful, even the cake was in tiers.",
            "What do you call a dinosaur with a extensive vocabulary? A thesaurus."
        };

        return messageArray[Random.Range(0, messageArray.Length)];
    }

}
