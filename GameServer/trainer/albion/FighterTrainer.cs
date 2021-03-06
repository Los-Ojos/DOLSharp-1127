/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using DOL.GS.PacketHandler;

namespace DOL.GS.Trainer
{
    /// <summary>
    /// Fighter Trainer
    /// </summary>
    [NPCGuildScript("Fighter Trainer", eRealm.Albion)] // this attribute instructs DOL to use this script for all "Fighter Trainer" NPC's in Albion (multiple guilds are possible for one script)
    public class FighterTrainer : GameTrainer
    {
        public override eCharacterClass TrainedClass => eCharacterClass.Fighter;

        // Item Template Ids
        private const string PracticeWeaponId = "practice_sword";
        private const string PracticeShieldId = "small_training_shield";

        public FighterTrainer() : base(eChampionTrainerType.Fighter)
        {
        }

        /// <summary>
        /// Interact with trainer
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
            {
                return false;
            }

            // check if class matches
            if (player.CharacterClass.ID == (int)TrainedClass)
            {
                // player can be promoted
                if (player.Level >= 5)
                {
                    player.Out.SendMessage(Name + " says, \"You must now seek your training elsewhere. Which path would you like to follow? [Armsman], [Paladin], or [Mercenary]?\"", eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                }
                else
                {
                    OfferTraining(player);
                }

                // ask for basic equipment if player doesnt own it
                if (player.Inventory.GetFirstItemByID(PracticeWeaponId, eInventorySlot.MinEquipable, eInventorySlot.LastBackpack) == null)
                {
                    player.Out.SendMessage(Name + " says, \"Do you require a [practice weapon]?\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                }

                if (player.Inventory.GetFirstItemByID(PracticeShieldId, eInventorySlot.MinEquipable, eInventorySlot.LastBackpack) == null)
                {
                    player.Out.SendMessage(Name + " says, \"Do you require a [training shield]?\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                }
            }
            else
            {
                CheckChampionTraining(player);
            }

            return true;
        }

        /// <summary>
        /// Talk to trainer
        /// </summary>
        /// <param name="source"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public override bool WhisperReceive(GameLiving source, string text)
        {
            if (!base.WhisperReceive(source, text))
            {
                return false;
            }

            if (!(source is GamePlayer player))
            {
                return false;
            }

            switch (text) {
                case "Armsman":
                    if (player.Race == (int)eRace.Avalonian || player.Race == (int)eRace.Briton || player.Race == (int)eRace.HalfOgre || player.Race == (int)eRace.Highlander || player.Race == (int)eRace.Inconnu || player.Race == (int)eRace.Saracen || player.Race == (int)eRace.AlbionMinotaur)
                    {
                        player.Out.SendMessage(Name + " says, \"Ah! An Armsmen is it? Good solid fighters they are! Their fighting prowess is a great asset to Albion. To become an armsman you must enlist with the Defenders of Albion.\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                    }
                    else {
                        player.Out.SendMessage(Name + " says, \"The path of an Armsman is not available to your race. Please choose another.\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                    }

                    return true;
                case "Mercenary":
                    if (player.Race == (int)eRace.Avalonian || player.Race == (int)eRace.Briton || player.Race == (int)eRace.HalfOgre || player.Race == (int)eRace.Highlander || player.Race == (int)eRace.Inconnu || player.Race == (int)eRace.Saracen || player.Race == (int)eRace.AlbionMinotaur)
                    {
                        player.Out.SendMessage(Name + " says, \"You wish to become a Mercenary do you? Roguish fighters in nature, solid warriors in battle, their ability to quickly evade enemy attacks has made them a valuable asset to the Guild of Shadows.\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                    }
                    else {
                        player.Out.SendMessage(Name + " says, \"The path of a Mercenary is not available to your race. Please choose another.\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                    }

                    return true;
                case "Paladin":
                    if (player.Race == (int)eRace.Avalonian || player.Race == (int)eRace.Briton || player.Race == (int)eRace.Highlander || player.Race == (int)eRace.Saracen) {
                        player.Out.SendMessage(Name + " says, \"You wish to be a defender of the faith I take it? Many a Paladin has led our fighters into battle with victory not far behind. Their never-ending sacrifice proves that the Church of Albion will remain for many centuries!\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                    }
                    else {
                        player.Out.SendMessage(Name + " says, \"The path of a Paladin is not available to your race. Please choose another.\"",eChatType.CT_Say,eChatLoc.CL_PopupWindow);
                    }

                    return true;
                case "practice weapon":
                    if (player.Inventory.GetFirstItemByID(PracticeWeaponId, eInventorySlot.Min_Inv, eInventorySlot.Max_Inv) == null)
                    {
                        player.ReceiveItem(this,PracticeWeaponId);
                    }

                    return true;
                case "training shield":
                    if (player.Inventory.GetFirstItemByID(PracticeShieldId, eInventorySlot.Min_Inv, eInventorySlot.Max_Inv) == null)
                    {
                        player.ReceiveItem(this, PracticeShieldId);
                    }

                    return true;
            }

            return true;
        }
    }
}
