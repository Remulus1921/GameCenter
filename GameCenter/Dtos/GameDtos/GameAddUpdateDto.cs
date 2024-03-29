﻿namespace GameCenter.Dtos.GameDto
{

    public class GameAddUpdateDto
    {
        public string Name { get; set; }
        public string GameType { get; set; }
        public string Description { get; set; }
        public string Studio { get; set; }
        public string Rating { get; set; }
        public int Capacity { get; set; }
        public IFormFile Image { get; set; }
        public List<string> Platforms { get; set; }
    }
}
