using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlphabetController : ControllerBase
    {

        //One option is to first capture the user's input as a string variable and make it all lowercase. Then you
        //use string that contains all of the alphabets in order. Then you take loop through the input and compare each 
        //character to the alphabets in the array. That will give you the position of the input in the array. You can then 
        //use ints to find the shortest possible path from the current letter to the next letter. 


        //[HttpPost]
        //[Route("GetTime")]
        //public async Task<IActionResult> GetTime(string word)
        //{
        //    //string normalizedWord = word.ToLower();
        //    //string alphabets = "abcdefghijklmnopqrstuvwxyz";

        //    //string newAlphabtes = alphabets.Substring(0, 10) + alphabets.Substring(10, 16);

        //    //double normalMovement = 5;
        //    //double doubleOrStart = 2.5;

        //    //double timeTaken = 0;

        //    //for (int i = 0; i < normalizedWord.Length; i++)
        //    //{
        //    //    if (normalizedWord[0] == 'a')
        //    //    {
        //    //        timeTaken += doubleOrStart;
        //    //    }

        //    //    //else if (normalizedWord[i - 1] == normalizedWord[i])
        //    //    //{
        //    //    //    timeTaken += doubleOrStart;
        //    //    //}

        //    //    else
        //    //    {
        //    //        int index = normalizedWord[i] - (normalizedWord[i] - 1);
        //    //        timeTaken += (index * normalMovement);
        //    //    }
        //    //}

        //    //for (int i = 0; i < normalizedWord.Length; i++)
        //    //{
        //    //    if (normalizedWord[0] == 'a')
        //    //    {
        //    //        timeTaken += 2.5;
        //    //    }

        //    //    for (int j = 0; j < alphabets.Length; j++)
        //    //    {
        //    //        if (normalizedWord[i] == alphabets[j])
        //    //        {
        //    //            int positionInAlphabets = j;


        //    //        }
        //    //    }
        //    //}

        //    //return Ok(newAlphabtes);
        //}


    }
}
