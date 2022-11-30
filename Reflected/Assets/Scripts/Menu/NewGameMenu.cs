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
        "there are different types of chest each with a specific tier of loot",
        "do NOT anger George",
        "to kill enemies, try hitting them with your sword", 
        "very time you kill and enemy, its soul is toutured in hell for a thousand years",
        "are we the baddies?",
        "gardening-simulator 2022",
        "use mirror-shard to upgrade your tech-tree in the start menu",
        "using your whirlwind attack will make you fly for a short period",
        "are these tips actually usefull?",
        "the fire upgrade for you sword will give it an damage-over-time effect",
        "the freeze upgrade for your sword will slow down enemies when you hit them",
        "the life-regeneration upgrade for your sword will heal you each time you hit an enemy",
        "the laser upgrade for your sword will make it possible to cut down trees",
        "UwU",
        "the legend has it that there is a secret second boss",
        "to swap dimension you need dimension charges",
        "now with extra enemies!",
        "you do not need to eat or sleep, reflected is your life now",
        "love is temporary, reflected is eternal",
        "this game was not made in a sweat-shop",
        "did you know Filip is an anagram for Liipf",
        "When you are not in the room, the enemies in there worship Kevin",
        "Emil once finished the game without killing a single enemy",
        "Pär is shor for Päron",
        "Marcus is mining bitcoin on your computer in the background while you play",
        "If you pirate the game, Valter will knock on your door. He is the danger",
        "If you play the sound of George in reverse, you will hear that it is actually Gustaf singing",
        "During development the team consumed a collected amount of coffee weighing the same as a small elephant",
        "If you do not swap dimension every now and then, a flasher will start chasing you"
    };

    protected override void Start()
    {
        tipText = GameObject.Find("Tip-text").GetComponent<TextMeshProUGUI>();
        tipText.text = "Usefull Tip: " + tipList[Random.Range(0, tipList.Length - 1)];
    }

    protected override void Update()
    {
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
