using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameMenu : Menu
{
    TextMeshProUGUI tipText;
    string[] tipList = new string[]
    {
        "try not to die",
        "collected coins can be used in the shop",
        "there are different types of chests, each with a specific tier of loot",
        "do NOT anger George",
        "to kill enemies, try hitting them with your sword",
        "every time you kill an enemy, its soul is tortured in hell for a thousand years",
        "are we the baddies?",
        "gardening-simulator 2022",
        "use mirror-shards to upgrade your tech-tree in the Main Menu",
        "using your whirlwind attack will stop you from falling for for a short period",
        "are these tips actually useful?",
        "the fire upgrade for your sword will give it a damage-over-time effect",
        "the freeze upgrade for your sword will slow down enemies when you hit them",
        "the life-regeneration upgrade for your sword will heal you over time, each time you hit an enemy",
        "the laser upgrade for your sword will make it possible to cut down trees",
        "UwU",
        "legend has it that there is a secret second boss",
        "to swap between dimensions, you need dimension charges",
        "now with extra enemies!",
        "you do not need to eat or sleep, Reflected is your life now",
        "love is temporary, Reflected is eternal",
        "this game was not made in a sweat-shop",
        "did you know Filip is an anagram for Liipf",
        "when you are not in the room, the enemies in there worship Kevin",
        "Emil once finished the game without killing a single enemy",
        "P�r is short for P�ron",
        "Marcus is mining bitcoin on your computer in the background while you play",
        "if you pirate the game, Valter will knock on your door. He is the danger",
        "if you play the sound of George in reverse, you will hear that it is actually Gustaf singing",
        "during development the team consumed a collected amount of coffee weighing the same as a small elephant",
        "if the chests have different tiers of loot it's just a way to trick you!",
        "if you do not swap dimension every now and then, a flasher will start chasing you",
        "the sun blossom may look friendly compared to the other creatures, but that is all a ruse",
        "beware of the poisonous gas that the bud will bombard you with",
        "the sun blossoms spores may not hurt initially, but they will poison and slow you down over time",
        "watch out for the hermit king, as he is both tougher and stronger than the standard shells",
        "clearing a room of enemies will reward you with a dimension charge",
        "ranged enemies will be scared of you when you are close to them",
        "keep moving",
        "do not get surrounded",
        "if you need help, consult the tutorial dummy... dummy!",
        "if the sun blossom starts charging its attack, you better run or finish it before it explodes",
        "need defensive upgrades? swap to the mirror dimension",
        "need offensive upgrades? swap to the true dimension"
    };

    protected override void Start()
    {
        tipText = GameObject.Find("Tip-text").GetComponent<TextMeshProUGUI>();
        tipText.text = "Useful Tip: " + tipList[Random.Range(0, tipList.Length)];
    }

    protected override void Update()
    {
        if (menuManager.ActiveMenu != this)
            return;

        if (playerController.Back())
            OnClick_Back();
    }

    protected override void OnEnable()
    {

    }

    protected override void OnDisable()
    {

    }

    public void OnClick_Back()
    {
        menuManager.SwapMenu(this, Type.Main);
    }
}
