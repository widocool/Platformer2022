namespace Exercice
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Assertions;

	public class Exercices_Beginner : MonoBehaviour
	{
		private void Awake()
		{
			Exercices();
		}

		private void Exercices()
		{
			Exercice01_Swap();
			Exercice02_MinMax();
			Exercice03_PrintBonjour();
			Exercice04_MinMaxArrays();
			Exercice05_ConcatStr();
		}

		#region Exercice01 Swap
		// Compléter la fonction Swap pour que les Debug Log renvoient la bonne information.
		private void Exercice01_Swap()
		{
			int a = 0;
			int b = 1;
			// This should print "Exercice01_Swap : 0 1"
			Debug.LogFormat("Exercice01_Swap : {0} {1}", a, b);
			Swap(a, b);
		}

		private void Swap(int firstValue, int secondValue)
		{
			// Insert code here



			// This should print "Exercice01_Swap : 1 0"
			Debug.LogFormat("Exercice01_Swap : {0} {1}", firstValue, secondValue);
			Assert.IsTrue(firstValue == 1 && secondValue == 0);
		}
		#endregion

		#region Exercice02_MinMax
		//Compléter deux fonctions, l'une renvoyant le plus petit entier entre deux valeurs, l'autre le plus grand.
		private void Exercice02_MinMax()
		{
			int resultMin = Min(5, 3);
			int resultMax = Max(5, 3);

			// This should print "Exercice02_MinMax : 3 5"
			Debug.LogFormat("Exercice02_MinMax : {0} {1}", resultMin, resultMax);
			Assert.IsTrue(resultMin == 3 && resultMax == 5);
		}

		private int Min(int a, int b)
		{
			// remove this line and add your code
			return 0;
		}

		private int Max(int a, int b)
		{
			// remove this line and add your code
			return 0;
		}
		#endregion

		#region Exercice03_PrintBonjour
		//Afficher "Bonjour" 5 fois avec une boucle.
		private void Exercice03_PrintBonjour()
		{
			// ...
		}
		#endregion

		#region Exercice04_MinMaxArrays
		//Créer deux fonctions, l'une renvoyant le plus petit entier d'un tableau d'entier, l'autre le plus grand. 

		private void Exercice04_MinMaxArrays()
		{
			int[] integers = new int[] { 4, 7, 2, 8, 1, 3, 8 };

			int resultMin = 0, resultMax = 0;
			// Uncomment this three lines and add the correct functions to compile
			//resultMin = MinArray(integers);
			//resultMax = MaxArray(integers);
			//Debug.LogFormat("Exercice04_MinMaxArrays : {0} {1}", resultMin, resultMax); // This should print "Exercice04_MinMaxArrays : 1 8"
			Assert.IsTrue(resultMin == 1 && resultMax == 8);
		}

		// Add your Min / Max functions here
		#endregion

		#region Exercice05_ConcatStr
		//Créer une fonction qui concatène ("ajoute l'un à l'autre") un message contenu dans un tableau de string avec un seul Debug.Log();

		private void Exercice05_ConcatStr()
		{
			string[] words = new string[] { "Bonjour", "à", "tous", "comment", "allez-vous", "?" };
			string result = string.Empty;

			// Uncomment this two lines and add the correct function to compile
			//result = ConcateWordArray(words);
			//Debug.LogFormat("Exercice05_ConcatStr : {0}", result); // Print "Bonjour à tous comment allez-vous ?"
			Assert.IsTrue(string.Compare(result, "Bonjour à tous comment allez-vous ?") == 0);
		}
		#endregion
	} 
}