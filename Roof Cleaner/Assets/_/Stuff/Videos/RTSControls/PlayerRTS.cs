using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerRTS : MonoBehaviour {

    public static PlayerRTS Instance { get; private set; }

    private CharacterRTS selectedCharacter;

    private void Awake() {
        Instance = this;
    }

    public void SetSelectedCharacter(CharacterRTS selectedCharacter) {
        this.selectedCharacter?.SetSelectedGameObjectVisible(false);

        this.selectedCharacter = selectedCharacter;

        this.selectedCharacter?.SetSelectedGameObjectVisible(true);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Collider2D[] collider2DArray = Physics2D.OverlapPointAll(UtilsClass.GetMouseWorldPosition());

            // Deselect Character
            SetSelectedCharacter(null);

            foreach (Collider2D collider2D in collider2DArray) {
                CharacterRTS characterRTS = collider2D.GetComponent<CharacterRTS>();
                if (characterRTS != null && characterRTS.IsPlayer()) {
                    SetSelectedCharacter(characterRTS);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if (selectedCharacter != null) {
                Collider2D[] collider2DArray = Physics2D.OverlapPointAll(UtilsClass.GetMouseWorldPosition());

                bool doMoveAction = true;

                foreach (Collider2D collider2D in collider2DArray) {
                    CharacterRTS characterRTS = collider2D.GetComponent<CharacterRTS>();
                    if (characterRTS != null && !characterRTS.IsPlayer()) {
                        selectedCharacter.SetTarget(characterRTS);
                        doMoveAction = false;
                        break;
                    }
                }

                if (doMoveAction) {
                    selectedCharacter.SetTarget(null);
                    selectedCharacter.SetMovePosition(UtilsClass.GetMouseWorldPosition());
                }
            }
        }
    }

}
