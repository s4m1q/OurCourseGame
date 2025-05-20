using NUnit.Framework;
using UnityEngine;


namespace Test
{
    public class UnitTests
    {
        /*
        [Test]
        public void Player_Take_Damage()
        {
            //Arrange
            var player = new GameObject().AddComponent<PlayerController>();
            player.currentHealth = 100;

            //Act
            player.TakeDamage(20);

            //Assert
            Assert.AreEqual(20, player.currentHealth);
        }
        */
        [Test]
        public void Player_Add_LessStamina()
        {
            var player = new GameObject().AddComponent<PlayerController>();
            player.currentStamina = 100;
            player.maxStamina = 150;

            player.AddStamina(20);

            Assert.AreEqual(120, player.currentStamina);
        }

        [Test]
        public void Player_Add_MoreStamina()
        {
            var player = new GameObject().AddComponent<PlayerController>();
            player.currentStamina = 100;
            player.maxStamina = 110;

            player.AddStamina(20);

            Assert.AreEqual(110, player.currentStamina);
        }

        [Test]
        public void Player_Add_LessHP()
        {
            var player = new GameObject().AddComponent<PlayerController>();
            player.currentHealth = 100;
            player.maxHealth = 150;

            player.AddHP(20);

            Assert.AreEqual(120, player.currentHealth);
        }


        [Test]
        public void Player_Add_MoreHP()
        {
            var player = new GameObject().AddComponent<PlayerController>();
            player.currentHealth = 100;
            player.maxHealth = 110;

            player.AddHP(20);

            Assert.AreEqual(110, player.currentHealth);
        }



        


    }
}

