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
        "use mirror-shards to upgrade your tech-tree in the start menu",
        "using your whirlwind attack will stop you from falling for for a short period",
        "are these tips actually useful?",
        "the fire upgrade for your sword will give it an damage-over-time effect",
        "the freeze upgrade for your sword will slow down enemies when you hit them",
        "the life-regeneration upgrade for your sword will heal you each time you hit an enemy",
        "the laser upgrade for your sword will make it possible to cut down trees",
        "UwU",
        "the legend has it that there is a secret second boss",
        "to swap between dimensions, you need dimension charges",
        "now with extra enemies!",
        "you do not need to eat or sleep, Reflected is your life now",
        "love is temporary, Reflected is eternal",
        "this game was not made in a sweat-shop",
        "did you know Filip is an anagram for Liipf",
        "when you are not in the room, the enemies in there worships Kevin",
        "Emil once finished the game without killing a single enemy",
        "P�r is short for P�ron",
        "Marcus is mining bitcoin on your computer in the background while you play",
        "If you pirate the game, Valter will knock on your door. He is the danger",
        "If you play the sound of George in reverse, you will hear that it is actually Gustaf singing",
        "During development the team consumed a collected amount of coffee weighing the same as a small elephant",
        "If the chests have different tiers of loot it's just a way to trick you!",
        "If you do not swap dimension every now and then, a flasher will start chasing you"
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
