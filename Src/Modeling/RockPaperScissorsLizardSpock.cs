﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using RT.KitchenSink;
using RT.Util.ExtensionMethods;
using RT.Util.Geometry;

namespace KtaneStuff.Modeling
{
    using static Md;

    static class RockPaperScissorsLizardSpock
    {
        private static string[] Responses = new[] { "Rock", "Paper", "Scissors", "Lizard", "Spock" };

        public static void GenerateModels()
        {
            var svgs = new Dictionary<string, string>();
            svgs.Add("Rock", @"<svg xmlns='http://www.w3.org/2000/svg' width='215pt' height='215pt' viewBox='0 0 215 215'><path d='M211.875 107.825C211.875 49.8768 165.1008 2.9 107.4 2.9 49.6992 2.9 2.9 49.8768 2.9 107.825S49.6992 212.75 107.4 212.75s104.475-46.9768 104.475-104.925L194.55 86.8c4.92 19.02 2.92 39.66-5 57.6-8.81 19.85-25.1 36.31-44.9 45.25-14.51 6.69-30.8 8.99-46.65 7.5-19.58-2.15-38.41-10.86-52.55-24.6-14.02-13.35-23.45-31.435-26.4-50.575-2.47-16.7-.5-34.185 6.35-49.675C35.24 49.23 55.41 30.9 79.2 23c9.20562-3.10375 18.86917-4.5973 28.55-4.55 14.1489.06915 28.30313 3.4303 40.95 9.825-6.51 6.3-13.16 12.455-19.6 18.825-1.73 2.17-4.635 1.95-7.125 2.15-10.97.48-21.845 2.225-32.625 4.225-9.94 2.01-20.085 4.02-29.125 8.8-4.05 2.36-8.67 5.57-8.9 10.75-1.41 14.29-2.325 28.63-2.625 43 .07 2.37-.24 5.015 1.2 7.075 1.95 2.9 4.85 4.94 7.45 7.2.26 3 .17 6.16 1.45 8.95 1.79 3.15 4.975 5.135 8.075 6.825 4.13 1.78 5.29 6.72 9.05 9 4.22 3.53 9.925 3.875 15.175 3.475 3.45-.51 5.945 2.29 8.825 3.65 2.5 1.44 5.485.85 8.225.9 1.45 2.28 2.14 5.73 4.95 6.7 12.44-3.85 24.705-8.34 37.025-12.6 3.23-.87 4.785-3.99 6.425-6.6 3.43-5.93 7.08-11.745 10.25-17.825 1.36-2.51 1.35-5.43 1.55-8.2.28-5.77.76-11.54 1.15-17.3 8.33-6.86 16.55-13.845 25.05-20.475' class='half' fill='#ccc'/><path d='M205.025 63.2C194.005 38.66 173.33 18.71 148.5 8.4 135.53 2.82 121.425.48 107.375 0h-.275C92.65.38 78.18 3 64.9 8.8 41.11 18.89 21.28 37.935 10.2 61.275 4.55 72.855.86083 85.47497 0 98.3c-.42057 6.2659-.4602 12.5619 0 18.825 1.92132 26.14775 14.515 51.245 33.775 69.075 17.94 16.9 41.84 27.11 66.4 28.8h13.95c21.65-1.56 42.805-9.57 59.775-23.15 20.56-16.18 34.895-40.12 39.175-65.95 3.68-21.06.85-43.26-8.05-62.7l-2.975 8.05c6.62 17.36 8.705 36.535 5.225 54.825-4.33 24.5-18.16 47.165-37.95 62.225-17.67 13.68-39.99 21.3-62.35 21-22.69 0-45.265-8.135-62.875-22.425-16.21-13.02-28.315-31.145-33.925-51.175-6.08-21.1-4.98-44.18 3.1-64.6 9.8-25.3 30.305-46.19 55.375-56.55 12.27687-5.24062 25.5961-7.83422 38.925-7.825 19.4807.01347 38.95375 5.55344 55.175 16.425.32 1.34-.99 2.045-1.75 2.875-9.45 8.57-18.755 17.28-28.425 25.6-2.73 2.82-6.85 2.605-10.45 3.025-19.52 1.91-39.185 4.795-57.575 11.925-4.11 1.46-6.93 5.5-8.1 9.55-1.24 14.35-2.03 28.755-3 43.125 1.71 1.84 3.47 3.63 5.25 5.4 2.18-12.51 3.4-25.23 5.95-37.65l2-.625c-1.19 14.88-2.855 29.72-4.175 44.6-.83 5.54 4.7 9.03 8.65 11.75 1.79-14.08 3.575-28.185 5.875-42.175.54-.16 1.635-.45 2.175-.6-1.23 15.82-2.85 31.62-3.8 47.45 2.94 3.96 7.54 5.805 12.2 6.825-.37-2.09-.635-4.215-.325-6.325 1.38-12.47 2.72-24.95 4.25-37.4.55.01 1.665.015 2.225.025-.6 13.53-1.325 27.07-1.925 40.6 4.83 3.63 10.61 8.295 17.05 6.275 7.42-1.17 13.185-6.235 19.175-10.325 1.43-1.21 3.52-2.12 3.9-4.15 1.12-4.62 1.91-9.335 2.2-14.075-1.26-4.16-3.905-7.73-5.825-11.6-1.87 3.38-4.56 6.18-7.85 8.2 1.81 3.35 4.11 6.805 3.9 10.775-1.58-2.42-3.065-4.905-4.475-7.425-.62 5.64-4.845 10.755-9.925 13.025 1.79-3.78 4.85-6.975 5.8-11.125 1.04-6.4-.075-12.8-.425-19.2 2.96 2.54 3.18 6.635 3.6 10.225 3.2-1.54 5.965-3.81 8.225-6.55-.83-1.57-1.625-3.125-2.375-4.725 3.87 3.43 6.7 7.785 10 11.725 1.45 1.92 3.215 3.705 4.125 5.975-.42 4.97-2.03 9.73-3.05 14.6-.65 2.13-.685 4.88-2.825 6.15-6.24 4.29-12.865 7.965-19.675 11.275.43.95.845 1.915 1.275 2.875 11.87-4.06 23.65-8.42 35.4-12.8 3.2-5.88 6.365-11.76 9.625-17.6 5.52-8.21 2.925-18.67 5.275-27.8.82-1.69 2.305-2.935 3.625-4.225 8.96-8.12 18.16-15.935 27.45-23.675 2.14-1.76 4.14-3.68 6.05-5.7'/></svg>");
            svgs.Add("Paper", @"<svg xmlns='http://www.w3.org/2000/svg' width='215pt' height='215pt' viewBox='0 0 215 215'><path d='M2.025 106.6c0 58.50478 46.9768 105.925 104.925 105.925S211.875 165.10478 211.875 106.6C211.875 48.0952 164.8982.675 106.95.675S2.025 48.0952 2.025 106.6L20.7 86.3c7.56 5.24 15.34 10.575 21.2 17.775 4.28 7.54 6.54 15.995 10.1 23.875 1.3 3.17 4.21 5.2 6.8 7.25 4.2 3.14 8.315 6.39 12.575 9.45 8.42 6.08 15.495 13.84 24.075 19.7 2.69 1.97 5.91-.185 8.15-1.825 4.6-4.04 7.63-11.095 4.85-16.975-2.61-5.49-7.525-9.275-11.575-13.625 9.57 3.27 16.445 11.155 25.175 15.925 7.51 4.06 15.59 7.27 24.05 8.6 5.64.77 12.56-.145 15.8-5.425 1.53-2 1.07-4.565.9-6.875 5.53-.15 11.37-1.97 14.9-6.45 2.1-2.87 3.685-6.31 3.875-9.9-1.16-1.62-3.34-1.995-5-2.925 2.24-2.85 5.205-6.025 4.725-9.925-.38-3.22-3.47-4.715-5.85-6.325 2.44-2.86 4.795-6.6 3.375-10.5-1.59-2.36-4.155-3.785-6.375-5.475-11.04-7.68-22.125-15.3-33.325-22.75-6.38-4.03-12.405-8.82-19.575-11.4-14.56-5.45-29.38-10.135-44.15-14.975-6.28-2.08-11.58-6.18-16.85-10.05 8.73-6.7 19.435-10.515 30.075-12.975 6.12375-1.3275 12.3807-1.99916 18.65-2.025 10.44883-.04307 20.91875 1.74375 30.725 5.375 20.34 7.21 37.8 22.12 48.1 41.1 9.85 17.75 13.36 38.955 9.7 58.925-3.57 19.61-13.68 38.07-28.75 51.2-14.74 13.36-34.185 21.185-53.975 22.475-22.84 1.44-46.105-6.21-63.475-21.15-25.82-21.21-37.39-57.84-28.9-90.1' class='half' fill='#ccc'/><path d='M12.2 73.9c10.86 7.24 22.33 14.005 31.2 23.725 7.38 8.88 10.295 20.38 15.675 30.4 9.4 7.89 19.51 14.985 28.6 23.275 3.44 2.92 6.505 6.37 10.625 8.35 4.08-3.2 6.36-9.05 4.05-13.95-3.43-6.61-8.99-11.695-14.2-16.875-3.05-3.02-6.28-5.935-10.2-7.775.97-.7 2.155-.68 3.225-.25 12.15 4.08 24.005 9.235 34.725 16.325 9.02 6.33 19.3 11.04 30.15 13.15 3.67.62 8.395 1.135 11.025-2.125 1.45-2.69 1.51-7.11-1.8-8.45-13.96-8.42-28.015-16.69-42.075-24.95.66-1.09 1.58-1.29 2.55-.5 13.98 7.73 27.835 15.74 42.175 22.8 3.04 1.77 6.645 1.67 10.025 1.3 4.82-.21 8.975-4.84 8.975-9.6-18.8-9.48-37.125-19.92-55.825-29.6.35-1.53.395-3.32 1.775-4.35.47 1.44.305 3.53 2.125 4.05 15.34 7.76 30.675 15.55 46.075 23.2 2.01 1.35 3.425-1.15 4.375-2.55.97-1.92 1.79-5.095-.55-6.375-15.89-9.73-32.1-18.96-47.85-28.95.23-2.12.32-4.285 1.15-6.275.51 1.85.905 3.72 1.275 5.6 13.87 7.57 27.76 15.09 41.55 22.8 1.69-1.75 2.525-4.03 2.625-6.45-16.91-11.29-33.29-23.455-50.95-33.575-17.67-7.23-36.13-12.325-54.15-18.575-9.22-4.56-17.48-11.07-24.6-18.45C61.63 14.93 84.235 6.905 106.975 6.725c24.67-.14 49.265 9.105 67.675 25.525 16.72 14.6 28.255 34.975 32.375 56.775 3.93 19.6 1.615 40.325-6.275 58.675-9.35 22.23-27 40.835-48.7 51.375-19.95 9.75-43.165 12.915-64.925 8.225-21.47-4.41-41.44-15.97-55.8-32.55-13.68-15.47-22.215-35.31-24.525-55.8-1.68-15.17.28-30.68 5.4-45.05l-2.025-12.6C4.535 72.83.8823 85.39722 0 98.175c-.43926 6.36152-.48325 12.76667 0 19.125 2.03847 26.82095 15.265 52.53 35.425 70.4 17.7 16.03 40.885 25.62 64.675 27.3h14.1c18.28-1.37 36.19-7.21 51.55-17.25 21.63-14.01 37.94-36.035 44.85-60.875 5.52-19.48 5.37-40.465-.4-59.875-7.95-26.92-26.87-50.405-51.6-63.725C143.03 4.515 125.18.48 107.4 0h-.45C92.53.4 78.1 3.015 64.85 8.825 41.06 18.915 21.245 37.95 10.175 61.3'/></svg>");
            svgs.Add("Scissors", @"<svg xmlns='http://www.w3.org/2000/svg' width='215pt' height='215pt' viewBox='0 0 215 215'><path d='M192.45 137.1c-5.5 17.2-16.975 32.24-31.375 43-16.28 12.12-36.85 18.25-57.1 17.4-20.97-.82-41.535-9.415-56.925-23.675-15.01-13.64-24.945-32.655-27.825-52.725-3.27-21.66 1.655-44.6 14.025-62.75 6.81-10.59 16.16-19.395 26.7-26.225 14.115-8.865 30.7425-13.63188 47.4-13.75 5.5525-.03938 11.105.435 16.575 1.45 29.81 5.2 56.345 26.67 67.075 55.05-5.66.54-11.33-.19-17-.15-1.65-.14-3.485.18-4.975-.7-8.76-6.17-17.87-11.945-27.65-16.375-3.9-1.89-8.67-1.995-12.45.275-5.84 3.26-10.875 7.76-16.175 11.8-2.78 1.99-4.715 5.035-7.675 6.725-21.52-6.15-42.78-13.18-64.15-19.85-5.14 1.84-6.43 7.64-7 12.45.21 4.93 4.73 7.99 8.25 10.75 9.29 5.99 19.77 9.725 29.85 14.125 6.52 2.91 13.445 4.945 19.675 8.475l.1 1.2c-5.09 1.55-10.47 1.525-15.7 2.275-15.92 2.01-31.93 3.55-47.85 5.5-5.27.87-6.02 7.4-5.35 11.7 1.16 4.13 4.455 8.42 9.075 8.65 12.85 1.66 25.87.48 38.75-.35.59 4.81 1.45 10.185 5.35 13.525 2.79 2.52 6.595 3.38 10.275 3.4-.89 5.63 1.715 11.97 7.225 14.15 6.89 2.48 14.41 2.06 21.55 1.15 5.1-.45 9-4.055 12.95-6.925 8.22 1.89 17.01 4.2 25.35 1.85 8.24-4.06 13.395-12.075 19.925-18.225 4.99-5.01 12.77-3 19.1-3.2l19.65-29.7c0-57.9482-47.03997-104.925-105.05-104.925C49.03996 2.475 2.025 49.4518 2.025 107.4S49.03996 212.325 107.05 212.325c58.01003 0 105.05-46.9768 105.05-104.925' fill='#ccc' class='half'/><path d='M212.925 126.75c3.92-21.36 1.115-43.885-7.925-63.625-9.7-21.46-26.72-39.485-47.55-50.475C142.13 4.29 124.71.47 107.35 0h-.325c-14.45.39-28.92 3.02-42.2 8.85C41.055 18.93 21.27 37.965 10.2 61.275 4.56 72.835.86975 85.40347 0 98.2c-.43173 6.352-.47802 12.7513 0 19.1 1.89367 25.1504 13.66 49.275 31.7 66.925 18.16 18.02 42.925 29.025 68.425 30.775H114.2c21.85-1.6 43.23-9.745 60.25-23.575 20.03-16 34.075-39.395 38.475-64.675l-56.725-10.6c-.17.52-.505 1.54-.675 2.05-6.74.39-14.04-2.045-18.1-7.675-5.08-7.02-5.69-16.81-1.6-24.45-2.51.38-5 .83-7.5 1.3-.05 5.38-.19 10.875-1.7 16.075-2.17 5.54-8.195 8.27-13.775 8.95-.55-.27-1.675-.82-2.225-1.1-1.86-4.55-2.145-9.5-3.475-14.2-1.24-5.57-3.33-11.015-3.45-16.775C83.03 74.285 62.265 68.51 41.825 61.7c-1.95 2.7-2.935 6.135-2.125 9.425 1.34 2.72 4.415 3.895 6.825 5.475 16.48 9.05 33.765 16.515 50.975 24.075.34 1.79.65 3.785-.5 5.375-.96 1.77-3.285 1.555-4.975 1.975-21.36 2.55-42.7 5.335-64.1 7.575-.51 3.83-.175 8.645 3.425 10.925 5.37 1.71 11.125 1.09 16.675 1.25 22.36-.08 44.655-1.985 66.875-4.325 5.39-.5 11.305-1.23 16.275 1.45 2.89 1.95 2.925 5.905.875 8.475 4.3 1.05 9.12 4.66 8.05 9.65-1.79 3.66-4.775 6.55-7.625 9.4 6.58 1.43 13.64 3.365 20.25 1.175 8.53-4.06 13.485-12.55 20.325-18.7 1.33-1.16 3.17-1.365 4.85-1.625 9.28-1.1 18.65-.82 27.95 0-5.51 21.74-18.6 41.46-36.4 55.1-18.23 14.06-41.425 21.505-64.425 20.875-24.34-.69-48.365-10.435-66.125-27.125-13.18-12.14-22.95-27.895-28.05-45.075-6.58-21.29-5.62-44.76 2.5-65.5 8.27-21.7 24.3-40.27 44.5-51.7 15.12875-8.62062 32.52325-13.19136 49.925-13.225 4.0158-.00776 8.03687.24313 12.025.725 24.74 2.94 48.28 15.325 64.55 34.225 9.76 11.18 16.885 24.575 21.025 38.825-11.15-.59-22.34-.585-33.45-1.675-2.33-.29-4.895-.365-6.825-1.875-7.27-5.27-14.935-10.01-23.075-13.8-3.02-1.23-6.435-2.92-9.675-1.45-8.23 3.34-14.78 9.59-20.9 15.85-1.55 1.85-4.33 3.48-4.1 6.2 1.25 8.2 4.145 16.04 5.625 24.2 4.25-.2 8.57-2.41 10.8-6.1 2.03-5.43 1.815-11.4 1.525-17.1 4.74-1.17 9.475-2.36 14.175-3.65-2.42 8.53-3.18 18.37 1.2 26.4 2.97 5.68 9.505 8.09 15.525 8.75'/><path d='M75.91 130.34c-1.21 5.52 1.55 12.41 7.65 13.31 7.13.72 14.56.63 21.39-1.76 6.38-2.1 12.68-4.44 18.88-6.99 2.2-1.01 5.07-2.09 5.45-4.84-.27-1.76-1.05-3.81-3-4.25-3.29-1.02-6.8-.46-10.17-.18-13.41 1.48-26.77 3.43-40.2 4.71zm15.68 17.26c.03 3.29-.69 7.37 1.86 9.95 5.71 2.79 12.34 2.46 18.54 2.41 2.45.14 4.77-.73 6.69-2.22 6.29-4.55 12.98-8.79 18.02-14.8-.47-3.87-3.78-6.85-7.68-6.88-12.26 4.37-24.11 11.21-37.43 11.54z'/></svg>");
            svgs.Add("Lizard", @"<svg xmlns='http://www.w3.org/2000/svg' width='215pt' height='215pt' viewBox='0 0 215 215'><path d='M145.025 93.2c-8.36 1.81-16.58 4.25-24.55 7.35-4.95 1.74-10.29 4.28-15.6 2.6-3.87-1.16-5.64-5.18-6.95-8.65 7.79625-3.03625 16.10178-5.31883 24.5-5.35 1.19974-.00446 2.3975.03375 3.6.125 6.41.95 12.56 3.135 19 3.925l3.025.45c5.82.64 12.965 1.63 17.475-3.05 3.35-3.91 8.475-7.01 9.175-12.55.06-2.07.46-4.855-1.5-6.225-2.66-1.81-5.755-2.845-8.625-4.275-15.04-6.86-30.25-13.36-45.6-19.5-1.85-.66-3.715-1.675-5.725-1.625-3.47.9-6.665 2.535-9.975 3.875-14.12 6.28-28.255 12.665-41.725 20.275-3.37 1.4-3.735 5.3-4.675 8.35-3.56 13.87-5.965 27.995-8.525 42.075-.69 5.66-2.43 11.275-1.95 17.025 1.61 13.73 4.24 27.315 6.05 41.025-16.89-12.67-28.6-31.755-33.05-52.325-3.48-16.96-2.24-34.97 4.2-51.1 8.35-21.94 25.925-40.11 47.375-49.55 11.26875-4.9625 23.58584-7.5084 35.9-7.55 7.3885-.02496 14.78.85 21.95 2.65 17.24 4.23 33.085 13.815 44.925 27.025 15.64 17.16 23.985 40.68 22.875 63.85-.7 18.63-7.52 36.97-19.15 51.55-16.27 21.04-42.42 33.845-69 34.125L106.95 212.1c57.70256 0 104.475-46.69613 104.475-104.275 0-57.57887-46.77244-104.25-104.475-104.25-57.70256 0-104.5 46.67113-104.5 104.25S49.24744 212.1 106.95 212.1l1.525-14.375c-1.96-11.13-3.46-22.365-3.75-33.675.02-5.04.575-10.46 3.825-14.55 4.67-6.54 12.79-9.005 18.3-14.625 3.89-3.79 7.89-7.485 12.15-10.875 6.22-5.36 13.625-9.09 19.925-14.35-.53-3.35-1.03-6.825-2.8-9.775-1.66-3.16-5.215-4.455-8.075-6.225' class='half' fill='#ccc'/><path d='M100.1 215c4.619.27995 9.406.27995 14.025 0 18.8-1.37 37.225-7.535 52.875-18.075 21.24-14.18 37.155-36.155 43.825-60.825 6.48-23.56 4.665-49.335-5.275-71.675-9.52-21.83-26.54-40.255-47.55-51.475C142.57 4.42 124.965.48 107.425 0h-.375C85.23.36 63.415 6.84 45.575 19.55 19.755 37.37 2.25044 66.9744 0 98.25c-.45095 6.26713-.4606 12.58357 0 18.85 2.0109 27.35762 15.67 53.54 36.45 71.5 17.56 15.49 40.32 24.75 63.65 26.4l3.35-5.5c-1.84-10.42-3.75-20.875-4.1-31.475-.42-9.41-1.17-19.325 2.7-28.175 1.73-3.9 3.965-8.005 7.975-9.925 8.77-4.42 16.155-11.065 22.975-18.025 6.17-6.03 13.89-10.055 21.15-14.575-1.22-3.52-3.12-7.205-6.75-8.725-3.3-1.74-7.09-.53-10.4.55-9.2 3.34-17.99 8.75-28 9.15-9.01.14-15.875-8.575-16.025-17.125 12.85-5.75 27.68-9.19 41.4-4.4 8.22 2.76 17.255 4.325 25.825 2.325 4.27-1.16 4.765-6.23 4.075-9.9-14.03-5.15-28.115-10.12-42.225-15.05-1.55-.51-3.19-1.13-4.85-.75-12.89 2.8-25.56 6.575-38.45 9.375.02-.51.07-1.53.1-2.05 12.64-4.45 25.41-8.685 38.25-12.525 9.6 2.86 18.84 6.91 28.3 10.25 7.2 2.78 14.53 5.225 21.6 8.325.57 1.73 1.15 3.455 1.75 5.175.74-2.42 1.22-4.91 1.2-7.45-17.89-7.68-35.66-15.67-53.8-22.75-1.68-.77-3.59-.9-5.25 0-15.46 6.62-30.66 13.905-45.7 21.425-2.71.86-2.77 3.975-3.4 6.275-3.73 18.09-6.46 36.345-9.05 54.625-.75 4.66.535 9.295 1.175 13.875 2.59 17.01 5.705 33.925 8.375 50.925C30.77 183.675 8.47 150.97 6.1 116c-2.03-24.57 5.44-49.825 20.55-69.325 11.68-15.42 27.935-27.28 46.125-33.85 11.085-4.025 22.8675-6.03125 34.65-6 11.7825.03125 23.57 2.105 34.625 6.225 24.09 8.8 44.525 27.12 55.825 50.15 8.88 17.74 12.295 38.165 9.725 57.825-2.82 22.49-13.53 43.905-29.85 59.625-19.41 19.33-47 29.69-74.3 28.85'/></svg>");
            svgs.Add("Spock", @"<svg xmlns='http://www.w3.org/2000/svg' width='215pt' height='215pt' viewBox='0 0 215 215'><path d='M107.05 212.975c57.88634 0 104.825-46.9768 104.825-104.925S164.93634 3.125 107.05 3.125c-57.88635 0-104.8 46.9768-104.8 104.925s46.91365 104.925 104.8 104.925l16.1-116.925c-.34-.25-1.035-.75-1.375-1-1.65-14.68-3.485-29.33-5.275-44-.47-3.29-.445-6.705-1.525-9.875-2.32-4.27-7.975-6.215-12.475-4.475-2.71 1.37-4.2 4.165-5.85 6.575-7.76-2.88-17.145 4.345-16.375 12.575.88 22.07 1.43 44.145 2 66.225-.08 1.61.135 3.94-1.675 4.7-1.72 1.06-3.53-.44-5.05-1.2-8.66-5.63-17.44-11.37-27.2-14.95-5.68-2.18-12.99-1.12-17 3.7-1.47 1.96-2.965 5.1-.925 7.2 7.6 8.09 18.16 13.315 24.05 23.025 4.4 6.28 7.615 13.52 13.425 18.7 3.85 4.1 8.05 8.2 10.3 13.45 1.9 5.47 1.175 11.37 1.225 17.05-21.41-7.21-40.095-22.44-50.725-42.45-10.11-18.55-13.43-40.775-8.85-61.425C23.13 73.415 31.535 58.2 43.025 46.05c15.74-15.94 37.435-26.155 59.925-26.975 1.2175-.06438 2.45607-.11193 3.675-.125 18.28396-.19607 36.6125 5.35938 51.65 15.775C173.395 45.045 185.18 60.2 191.45 77.4c7.2 19.07 7.405 40.62.825 59.9-8.25 24.99-28.255 45.9-53.025 54.9-.51-5.63-1.77-12.135 1.95-16.975 5.33-7.65 10.965-15.74 12.075-25.25 1.85-14.24 3.575-28.505 5.925-42.675 1.68-10.2 4.895-20.11 6.125-30.4.37-3 .295-6.275-1.425-8.875-1.8-2.42-5.03-2.71-7.75-3.35.99-3.53 2.29-7.315 1.15-10.975-1.57-4.65-6.565-7.415-11.275-7.675-4.94.22-9.065 4.11-10.275 8.8-4.19 13.75-8.33 27.495-12.6 41.225' class='half' fill='#ccc'/><path d='M114.12482 215c21.4-1.55 42.355-9.38 59.225-22.7 19.74-15.36 33.91-37.82 38.95-62.35 4.57-21.33 2.42-44.055-6.15-64.125-9.41-22.45-26.71-41.46-48.2-52.9-15.42-8.53-33.02-12.445-50.55-12.925h-.425c-14.44.39-28.895 3.04-42.175 8.85-23.76 10.1-43.565 29.13-54.625 52.45-5.66 11.61-9.3376 24.26245-10.175 37.125-.40387 6.20353-.45048 12.44968 0 18.65 1.851 25.4766 13.86 49.925 32.25 67.675 18.1 17.72 42.61 28.51 67.85 30.25h14.025l-3.125-44.45c.69-5.51 2.39-11.075 1-16.625-1.73-9.96-9.61-17.675-18.45-21.875l.4-1.075-7.85-4.925c.11 3.97-4.575 5.91-7.925 4.9-4.84-1.38-8.745-4.775-13.125-7.125-6.25-3.46-12.295-7.57-19.175-9.7-4.61-1.47-9.47.915-11.95 4.875 6.37 6.02 13.545 11.18 19.725 17.4 5.43 6.03 9.29 13.26 14.45 19.5 5.03 5.34 10.34 10.6 14.05 17 2.7 4.56 2.48 10.05 2.95 15.15.28 6.45.805 12.9.425 19.35-22.89-5-43.8-18.3-58.1-36.85-14.27-18.2-21.9-41.44-21.3-64.55.63-23.35 9.49-46.48 25-64 15.67-18.15 38.035-30.355 61.775-33.725 4.5325-.675 9.0964-1.04 13.675-1.075 13.73578-.105 27.49 2.635 40.15 7.975 22.07 9.15 40.54 26.565 51.2 47.925 10.72 21.04 13.645 45.875 8.075 68.825-8 35.19-36.39 64.66-71.2 74.1-.68-8.65-1.83-17.405-.75-26.075.36-4.6 3.86-7.87 5.9-11.75 2.97-5.64 6.575-11.195 7.475-17.625 1.62-10.53 2.96-21.09 4.65-31.6 2.62-15.06 6.515-29.88 9.325-44.9 1.3-5.59-8.715-7.075-9.775-1.725-4.06 11.58-8.505 23.01-12.675 34.55 1.68 1.35 3.26 2.88 4.05 4.95-3.95-2.49-8.235-4.31-12.575-6 2.17-.27 4.345-.21 6.525-.05 5.29-14.17 10.68-28.29 15.45-42.65.86-2.72 2.025-5.74.825-8.55-1.78-3.81-7.075-5.025-10.575-2.875-1.62 1.15-2.25 3.165-2.95 4.925-4.81 13.73-8.865 27.72-13.525 41.5-.65 2.45-3.61 4.495-6 2.975-2.26-1.11-2.275-3.925-2.625-6.075-1.68-15.33-3.3-30.68-5.05-46-.37-2.97-.695-6.47-3.625-8.1-2.2-.76-5.205-.87-6.825 1.1-2.24 2.37-2.505 5.86-2.125 8.95 1.76 16.4 2.97 32.87 4.5 49.3 1.72.57 3.415 1.185 5.025 2.025-4.62.29-9.165 1.075-13.725 1.875 1.77-1.71 4.02-2.74 6.3-3.6-1.09-15.54-2.61-31.04-4-46.55-.37-2.06-.045-4.81-2.125-6.05-3.65-1.85-8.555.13-9.975 3.95-1.07 2.88-.53 6-.5 9 .66 21.7.485 43.4.675 65.1l7.85 4.925c9.28 2.65 18.715 9.13 20.725 19.15 1.25 6.87.315 14.575-3.675 20.425'/></svg>");

            foreach (var resp in Responses)
            {
                Console.WriteLine(resp);
                var svg = XDocument.Parse(svgs[resp]).Root;
                File.WriteAllText($@"D:\c\KTANE\RockPaperScissorsLizardSpock\Assets\Models\{resp}.obj", GenerateObjFile(GetMesh(svg, false, .1), resp));
                File.WriteAllText($@"D:\c\KTANE\RockPaperScissorsLizardSpock\Assets\Models\{resp}Half.obj", GenerateObjFile(GetMesh(svg, true, .1), resp + "Half"));
            }
            File.WriteAllText($@"D:\c\KTANE\RockPaperScissorsLizardSpock\Assets\Models\Disc.obj", GenerateObjFile(Disc(72, reverse: true), "Disc"));
        }

        public static void GenerateComponentSvg()
        {
            var pristineSvgs = new Dictionary<string, string>();
            pristineSvgs.Add("Rock", @"<path d='M107.4 2.9C49.6992 2.9 2.9 49.8768 2.9 107.825S49.6992 212.75 107.4 212.75s104.475-46.9768 104.475-104.925S165.1008 2.9 107.4 2.9zm.35 15.55c14.1489.06915 28.30313 3.4303 40.95 9.825-6.51 6.3-13.16 12.455-19.6 18.825-1.73 2.17-4.635 1.95-7.125 2.15-10.97.48-21.845 2.225-32.625 4.225-9.94 2.01-20.085 4.02-29.125 8.8-4.05 2.36-8.67 5.57-8.9 10.75-1.41 14.29-2.325 28.63-2.625 43 .07 2.37-.24 5.015 1.2 7.075 1.95 2.9 4.85 4.94 7.45 7.2.26 3 .17 6.16 1.45 8.95 1.79 3.15 4.975 5.135 8.075 6.825 4.13 1.78 5.29 6.72 9.05 9 4.22 3.53 9.925 3.875 15.175 3.475 3.45-.51 5.945 2.29 8.825 3.65 2.5 1.44 5.485.85 8.225.9 1.45 2.28 2.14 5.73 4.95 6.7 12.44-3.85 24.705-8.34 37.025-12.6 3.23-.87 4.785-3.99 6.425-6.6 3.43-5.93 7.08-11.745 10.25-17.825 1.36-2.51 1.35-5.43 1.55-8.2.28-5.77.76-11.54 1.15-17.3 8.33-6.86 16.55-13.845 25.05-20.475 4.92 19.02 2.92 39.66-5 57.6-8.81 19.85-25.1 36.31-44.9 45.25-14.51 6.69-30.8 8.99-46.65 7.5-19.58-2.15-38.41-10.86-52.55-24.6-14.02-13.35-23.45-31.435-26.4-50.575-2.47-16.7-.5-34.185 6.35-49.675C35.24 49.23 55.41 30.9 79.2 23c9.20562-3.10375 18.86917-4.5973 28.55-4.55z' color='#000' fill='#ccc' overflow='visible'/><path d='M107.1 0C92.65.38 78.18 3 64.9 8.8 41.11 18.89 21.28 37.935 10.2 61.275 4.55 72.855.86083 85.47497 0 98.3c-.42057 6.2659-.4602 12.5619 0 18.825 1.92132 26.14775 14.515 51.245 33.775 69.075 17.94 16.9 41.84 27.11 66.4 28.8h13.95c21.65-1.56 42.805-9.57 59.775-23.15 20.56-16.18 34.895-40.12 39.175-65.95 3.68-21.06.85-43.26-8.05-62.7-11.02-24.54-31.695-44.49-56.525-54.8C135.53 2.82 121.425.48 107.375 0h-.275zm.475 6.725c19.4807.01347 38.95375 5.55344 55.175 16.425.32 1.34-.99 2.045-1.75 2.875-9.45 8.57-18.755 17.28-28.425 25.6-2.73 2.82-6.85 2.605-10.45 3.025-19.52 1.91-39.185 4.795-57.575 11.925-4.11 1.46-6.93 5.5-8.1 9.55-1.24 14.35-2.03 28.755-3 43.125 1.71 1.84 3.47 3.63 5.25 5.4 2.18-12.51 3.4-25.23 5.95-37.65l2-.625c-1.19 14.88-2.855 29.72-4.175 44.6-.83 5.54 4.7 9.03 8.65 11.75 1.79-14.08 3.575-28.185 5.875-42.175.54-.16 1.635-.45 2.175-.6-1.23 15.82-2.85 31.62-3.8 47.45 2.94 3.96 7.54 5.805 12.2 6.825-.37-2.09-.635-4.215-.325-6.325 1.38-12.47 2.72-24.95 4.25-37.4.55.01 1.665.015 2.225.025-.6 13.53-1.325 27.07-1.925 40.6 4.83 3.63 10.61 8.295 17.05 6.275 7.42-1.17 13.185-6.235 19.175-10.325 1.43-1.21 3.52-2.12 3.9-4.15 1.12-4.62 1.91-9.335 2.2-14.075-1.26-4.16-3.905-7.73-5.825-11.6-1.87 3.38-4.56 6.18-7.85 8.2 1.81 3.35 4.11 6.805 3.9 10.775-1.58-2.42-3.065-4.905-4.475-7.425-.62 5.64-4.845 10.755-9.925 13.025 1.79-3.78 4.85-6.975 5.8-11.125 1.04-6.4-.075-12.8-.425-19.2 2.96 2.54 3.18 6.635 3.6 10.225 3.2-1.54 5.965-3.81 8.225-6.55-.83-1.57-1.625-3.125-2.375-4.725 3.87 3.43 6.7 7.785 10 11.725 1.45 1.92 3.215 3.705 4.125 5.975-.42 4.97-2.03 9.73-3.05 14.6-.65 2.13-.685 4.88-2.825 6.15-6.24 4.29-12.865 7.965-19.675 11.275.43.95.845 1.915 1.275 2.875 11.87-4.06 23.65-8.42 35.4-12.8 3.2-5.88 6.365-11.76 9.625-17.6 5.52-8.21 2.925-18.67 5.275-27.8.82-1.69 2.305-2.935 3.625-4.225 8.96-8.12 18.16-15.935 27.45-23.675 2.14-1.76 4.14-3.68 6.05-5.7 6.62 17.36 8.705 36.535 5.225 54.825-4.33 24.5-18.16 47.165-37.95 62.225-17.67 13.68-39.99 21.3-62.35 21-22.69 0-45.265-8.135-62.875-22.425-16.21-13.02-28.315-31.145-33.925-51.175-6.08-21.1-4.98-44.18 3.1-64.6 9.8-25.3 30.305-46.19 55.375-56.55 12.27687-5.24062 25.5961-7.83422 38.925-7.825z' fill='#000' />");
            pristineSvgs.Add("Paper", @"<path d='M106.95.675C49.0018.675 2.025 48.0952 2.025 106.6c0 58.50478 46.9768 105.925 104.925 105.925S211.875 165.10478 211.875 106.6C211.875 48.0952 164.8982.675 106.95.675zm.325 17.8c10.44883-.04307 20.91875 1.74375 30.725 5.375 20.34 7.21 37.8 22.12 48.1 41.1 9.85 17.75 13.36 38.955 9.7 58.925-3.57 19.61-13.68 38.07-28.75 51.2-14.74 13.36-34.185 21.185-53.975 22.475-22.84 1.44-46.105-6.21-63.475-21.15-25.82-21.21-37.39-57.84-28.9-90.1 7.56 5.24 15.34 10.575 21.2 17.775 4.28 7.54 6.54 15.995 10.1 23.875 1.3 3.17 4.21 5.2 6.8 7.25 4.2 3.14 8.315 6.39 12.575 9.45 8.42 6.08 15.495 13.84 24.075 19.7 2.69 1.97 5.91-.185 8.15-1.825 4.6-4.04 7.63-11.095 4.85-16.975-2.61-5.49-7.525-9.275-11.575-13.625 9.57 3.27 16.445 11.155 25.175 15.925 7.51 4.06 15.59 7.27 24.05 8.6 5.64.77 12.56-.145 15.8-5.425 1.53-2 1.07-4.565.9-6.875 5.53-.15 11.37-1.97 14.9-6.45 2.1-2.87 3.685-6.31 3.875-9.9-1.16-1.62-3.34-1.995-5-2.925 2.24-2.85 5.205-6.025 4.725-9.925-.38-3.22-3.47-4.715-5.85-6.325 2.44-2.86 4.795-6.6 3.375-10.5-1.59-2.36-4.155-3.785-6.375-5.475-11.04-7.68-22.125-15.3-33.325-22.75-6.38-4.03-12.405-8.82-19.575-11.4-14.56-5.45-29.38-10.135-44.15-14.975-6.28-2.08-11.58-6.18-16.85-10.05 8.73-6.7 19.435-10.515 30.075-12.975 6.12375-1.3275 12.3807-1.99916 18.65-2.025z' color='#000' fill='#ccc' overflow='visible'/><path d='M106.95 0C92.53.4 78.1 3.015 64.85 8.825 41.06 18.915 21.245 37.95 10.175 61.3 4.535 72.83.8823 85.39722 0 98.175c-.43926 6.36152-.48325 12.76667 0 19.125 2.03847 26.82095 15.265 52.53 35.425 70.4 17.7 16.03 40.885 25.62 64.675 27.3h14.1c18.28-1.37 36.19-7.21 51.55-17.25 21.63-14.01 37.94-36.035 44.85-60.875 5.52-19.48 5.37-40.465-.4-59.875-7.95-26.92-26.87-50.405-51.6-63.725C143.03 4.515 125.18.48 107.4 0h-.45zm.025 6.725c24.67-.14 49.265 9.105 67.675 25.525 16.72 14.6 28.255 34.975 32.375 56.775 3.93 19.6 1.615 40.325-6.275 58.675-9.35 22.23-27 40.835-48.7 51.375-19.95 9.75-43.165 12.915-64.925 8.225-21.47-4.41-41.44-15.97-55.8-32.55-13.68-15.47-22.215-35.31-24.525-55.8-1.68-15.17.28-30.68 5.4-45.05 10.86 7.24 22.33 14.005 31.2 23.725 7.38 8.88 10.295 20.38 15.675 30.4 9.4 7.89 19.51 14.985 28.6 23.275 3.44 2.92 6.505 6.37 10.625 8.35 4.08-3.2 6.36-9.05 4.05-13.95-3.43-6.61-8.99-11.695-14.2-16.875-3.05-3.02-6.28-5.935-10.2-7.775.97-.7 2.155-.68 3.225-.25 12.15 4.08 24.005 9.235 34.725 16.325 9.02 6.33 19.3 11.04 30.15 13.15 3.67.62 8.395 1.135 11.025-2.125 1.45-2.69 1.51-7.11-1.8-8.45-13.96-8.42-28.015-16.69-42.075-24.95.66-1.09 1.58-1.29 2.55-.5 13.98 7.73 27.835 15.74 42.175 22.8 3.04 1.77 6.645 1.67 10.025 1.3 4.82-.21 8.975-4.84 8.975-9.6-18.8-9.48-37.125-19.92-55.825-29.6.35-1.53.395-3.32 1.775-4.35.47 1.44.305 3.53 2.125 4.05 15.34 7.76 30.675 15.55 46.075 23.2 2.01 1.35 3.425-1.15 4.375-2.55.97-1.92 1.79-5.095-.55-6.375-15.89-9.73-32.1-18.96-47.85-28.95.23-2.12.32-4.285 1.15-6.275.51 1.85.905 3.72 1.275 5.6 13.87 7.57 27.76 15.09 41.55 22.8 1.69-1.75 2.525-4.03 2.625-6.45-16.91-11.29-33.29-23.455-50.95-33.575-17.67-7.23-36.13-12.325-54.15-18.575-9.22-4.56-17.48-11.07-24.6-18.45C61.63 14.93 84.235 6.905 106.975 6.725z' fill='#000' />");
            pristineSvgs.Add("Lizard", @"<path d='M106.95 3.575c-57.70256 0-104.5 46.67113-104.5 104.25S49.24744 212.1 106.95 212.1s104.475-46.69613 104.475-104.275c0-57.57887-46.77244-104.25-104.475-104.25zm-.075 14.95c7.3885-.02496 14.78.85 21.95 2.65 17.24 4.23 33.085 13.815 44.925 27.025 15.64 17.16 23.985 40.68 22.875 63.85-.7 18.63-7.52 36.97-19.15 51.55-16.27 21.04-42.42 33.845-69 34.125-1.96-11.13-3.46-22.365-3.75-33.675.02-5.04.575-10.46 3.825-14.55 4.67-6.54 12.79-9.005 18.3-14.625 3.89-3.79 7.89-7.485 12.15-10.875 6.22-5.36 13.625-9.09 19.925-14.35-.53-3.35-1.03-6.825-2.8-9.775-1.66-3.16-5.215-4.455-8.075-6.225 5.82.64 12.965 1.63 17.475-3.05 3.35-3.91 8.475-7.01 9.175-12.55.06-2.07.46-4.855-1.5-6.225-2.66-1.81-5.755-2.845-8.625-4.275-15.04-6.86-30.25-13.36-45.6-19.5-1.85-.66-3.715-1.675-5.725-1.625-3.47.9-6.665 2.535-9.975 3.875-14.12 6.28-28.255 12.665-41.725 20.275-3.37 1.4-3.735 5.3-4.675 8.35-3.56 13.87-5.965 27.995-8.525 42.075-.69 5.66-2.43 11.275-1.95 17.025 1.61 13.73 4.24 27.315 6.05 41.025-16.89-12.67-28.6-31.755-33.05-52.325-3.48-16.96-2.24-34.97 4.2-51.1 8.35-21.94 25.925-40.11 47.375-49.55 11.26875-4.9625 23.58584-7.5084 35.9-7.55zm15.55 70.625c1.19974-.00446 2.3975.03375 3.6.125 6.41.95 12.56 3.135 19 3.925-8.36 1.81-16.58 4.25-24.55 7.35-4.95 1.74-10.29 4.28-15.6 2.6-3.87-1.16-5.64-5.18-6.95-8.65 7.79625-3.03625 16.10178-5.31883 24.5-5.35z' color='#000' fill='#ccc' overflow='visible'/><path d='M107.05 0C85.23.36 63.415 6.84 45.575 19.55 19.755 37.37 2.25044 66.9744 0 98.25c-.45095 6.26713-.4606 12.58357 0 18.85 2.0109 27.35762 15.67 53.54 36.45 71.5 17.56 15.49 40.32 24.75 63.65 26.4h14.025c18.8-1.37 37.225-7.535 52.875-18.075 21.24-14.18 37.155-36.155 43.825-60.825 6.48-23.56 4.665-49.335-5.275-71.675-9.52-21.83-26.54-40.255-47.55-51.475C142.57 4.42 124.965.48 107.425 0h-.375zm.375 6.825c11.7825.03125 23.57 2.105 34.625 6.225 24.09 8.8 44.525 27.12 55.825 50.15 8.88 17.74 12.295 38.165 9.725 57.825-2.82 22.49-13.53 43.905-29.85 59.625-19.41 19.33-47 29.69-74.3 28.85-1.84-10.42-3.75-20.875-4.1-31.475-.42-9.41-1.17-19.325 2.7-28.175 1.73-3.9 3.965-8.005 7.975-9.925 8.77-4.42 16.155-11.065 22.975-18.025 6.17-6.03 13.89-10.055 21.15-14.575-1.22-3.52-3.12-7.205-6.75-8.725-3.3-1.74-7.09-.53-10.4.55-9.2 3.34-17.99 8.75-28 9.15-9.01.14-15.875-8.575-16.025-17.125 12.85-5.75 27.68-9.19 41.4-4.4 8.22 2.76 17.255 4.325 25.825 2.325 4.27-1.16 4.765-6.23 4.075-9.9-14.03-5.15-28.115-10.12-42.225-15.05-1.55-.51-3.19-1.13-4.85-.75-12.89 2.8-25.56 6.575-38.45 9.375.02-.51.07-1.53.1-2.05 12.64-4.45 25.41-8.685 38.25-12.525 9.6 2.86 18.84 6.91 28.3 10.25 7.2 2.78 14.53 5.225 21.6 8.325.57 1.73 1.15 3.455 1.75 5.175.74-2.42 1.22-4.91 1.2-7.45-17.89-7.68-35.66-15.67-53.8-22.75-1.68-.77-3.59-.9-5.25 0-15.46 6.62-30.66 13.905-45.7 21.425-2.71.86-2.77 3.975-3.4 6.275-3.73 18.09-6.46 36.345-9.05 54.625-.75 4.66.535 9.295 1.175 13.875 2.59 17.01 5.705 33.925 8.375 50.925C30.77 183.675 8.47 150.97 6.1 116c-2.03-24.57 5.44-49.825 20.55-69.325 11.68-15.42 27.935-27.28 46.125-33.85 11.085-4.025 22.8675-6.03125 34.65-6z' fill='#000' />");
            pristineSvgs.Add("Scissors", @"<path d='M107.05 2.475C49.03996 2.475 2.025 49.4518 2.025 107.4S49.03996 212.325 107.05 212.325c58.01003 0 105.05-46.9768 105.05-104.925S165.06003 2.475 107.05 2.475zm.3 15.9c5.5525-.03938 11.105.435 16.575 1.45 29.81 5.2 56.345 26.67 67.075 55.05-5.66.54-11.33-.19-17-.15-1.65-.14-3.485.18-4.975-.7-8.76-6.17-17.87-11.945-27.65-16.375-3.9-1.89-8.67-1.995-12.45.275-5.84 3.26-10.875 7.76-16.175 11.8-2.78 1.99-4.715 5.035-7.675 6.725-21.52-6.15-42.78-13.18-64.15-19.85-5.14 1.84-6.43 7.64-7 12.45.21 4.93 4.73 7.99 8.25 10.75 9.29 5.99 19.77 9.725 29.85 14.125 6.52 2.91 13.445 4.945 19.675 8.475l.1 1.2c-5.09 1.55-10.47 1.525-15.7 2.275-15.92 2.01-31.93 3.55-47.85 5.5-5.27.87-6.02 7.4-5.35 11.7 1.16 4.13 4.455 8.42 9.075 8.65 12.85 1.66 25.87.48 38.75-.35.59 4.81 1.45 10.185 5.35 13.525 2.79 2.52 6.595 3.38 10.275 3.4-.89 5.63 1.715 11.97 7.225 14.15 6.89 2.48 14.41 2.06 21.55 1.15 5.1-.45 9-4.055 12.95-6.925 8.22 1.89 17.01 4.2 25.35 1.85 8.24-4.06 13.395-12.075 19.925-18.225 4.99-5.01 12.77-3 19.1-3.2-5.5 17.2-16.975 32.24-31.375 43-16.28 12.12-36.85 18.25-57.1 17.4-20.97-.82-41.535-9.415-56.925-23.675-15.01-13.64-24.945-32.655-27.825-52.725-3.27-21.66 1.655-44.6 14.025-62.75 6.81-10.59 16.16-19.395 26.7-26.225 14.115-8.865 30.7425-13.63188 47.4-13.75z' color='#000' fill='#ccc' overflow='visible'/><path d='M107.025 0c-14.45.39-28.92 3.02-42.2 8.85C41.055 18.93 21.27 37.965 10.2 61.275 4.56 72.835.86975 85.40347 0 98.2c-.43173 6.352-.47802 12.7513 0 19.1 1.89367 25.1504 13.66 49.275 31.7 66.925 18.16 18.02 42.925 29.025 68.425 30.775H114.2c21.85-1.6 43.23-9.745 60.25-23.575 20.03-16 34.075-39.395 38.475-64.675 3.92-21.36 1.115-43.885-7.925-63.625-9.7-21.46-26.72-39.485-47.55-50.475C142.13 4.29 124.71.47 107.35 0zm.75 6.625c4.0158-.00776 8.03687.24313 12.025.725 24.74 2.94 48.28 15.325 64.55 34.225 9.76 11.18 16.885 24.575 21.025 38.825-11.15-.59-22.34-.585-33.45-1.675-2.33-.29-4.895-.365-6.825-1.875-7.27-5.27-14.935-10.01-23.075-13.8-3.02-1.23-6.435-2.92-9.675-1.45-8.23 3.34-14.78 9.59-20.9 15.85-1.55 1.85-4.33 3.48-4.1 6.2 1.25 8.2 4.145 16.04 5.625 24.2 4.25-.2 8.57-2.41 10.8-6.1 2.03-5.43 1.815-11.4 1.525-17.1 4.74-1.17 9.475-2.36 14.175-3.65-2.42 8.53-3.18 18.37 1.2 26.4 2.97 5.68 9.505 8.09 15.525 8.75-.17.52-.505 1.54-.675 2.05-6.74.39-14.04-2.045-18.1-7.675-5.08-7.02-5.69-16.81-1.6-24.45-2.51.38-5 .83-7.5 1.3-.05 5.38-.19 10.875-1.7 16.075-2.17 5.54-8.195 8.27-13.775 8.95-.55-.27-1.675-.82-2.225-1.1-1.86-4.55-2.145-9.5-3.475-14.2-1.24-5.57-3.33-11.015-3.45-16.775C83.03 74.285 62.265 68.51 41.825 61.7c-1.95 2.7-2.935 6.135-2.125 9.425 1.34 2.72 4.415 3.895 6.825 5.475 16.48 9.05 33.765 16.515 50.975 24.075.34 1.79.65 3.785-.5 5.375-.96 1.77-3.285 1.555-4.975 1.975-21.36 2.55-42.7 5.335-64.1 7.575-.51 3.83-.175 8.645 3.425 10.925 5.37 1.71 11.125 1.09 16.675 1.25 22.36-.08 44.655-1.985 66.875-4.325 5.39-.5 11.305-1.23 16.275 1.45 2.89 1.95 2.925 5.905.875 8.475 4.3 1.05 9.12 4.66 8.05 9.65-1.79 3.66-4.775 6.55-7.625 9.4 6.58 1.43 13.64 3.365 20.25 1.175 8.53-4.06 13.485-12.55 20.325-18.7 1.33-1.16 3.17-1.365 4.85-1.625 9.28-1.1 18.65-.82 27.95 0-5.51 21.74-18.6 41.46-36.4 55.1-18.23 14.06-41.425 21.505-64.425 20.875-24.34-.69-48.365-10.435-66.125-27.125-13.18-12.14-22.95-27.895-28.05-45.075-6.58-21.29-5.62-44.76 2.5-65.5 8.27-21.7 24.3-40.27 44.5-51.7 15.12875-8.62062 32.52325-13.19136 49.925-13.225z' fill='#000' /><path d='M75.91 130.34c13.43-1.28 26.79-3.23 40.2-4.71 3.37-.28 6.88-.84 10.17.18 1.95.44 2.73 2.49 3 4.25-.38 2.75-3.25 3.83-5.45 4.84-6.2 2.55-12.5 4.89-18.88 6.99-6.83 2.39-14.26 2.48-21.39 1.76-6.1-.9-8.86-7.79-7.65-13.31zm15.68 17.26c13.32-.33 25.17-7.17 37.43-11.54 3.9.03 7.21 3.01 7.68 6.88-5.04 6.01-11.73 10.25-18.02 14.8-1.92 1.49-4.24 2.36-6.69 2.22-6.2.05-12.83.38-18.54-2.41-2.55-2.58-1.83-6.66-1.86-9.95z' fill='#000' />");
            pristineSvgs.Add("Spock", @"<path d='M107.05 3.125c-57.88635 0-104.8 46.9768-104.8 104.925s46.91365 104.925 104.8 104.925c57.88634 0 104.825-46.9768 104.825-104.925S164.93634 3.125 107.05 3.125zm-.425 15.825c18.28396-.19607 36.6125 5.35938 51.65 15.775C173.395 45.045 185.18 60.2 191.45 77.4c7.2 19.07 7.405 40.62.825 59.9-8.25 24.99-28.255 45.9-53.025 54.9-.51-5.63-1.77-12.135 1.95-16.975 5.33-7.65 10.965-15.74 12.075-25.25 1.85-14.24 3.575-28.505 5.925-42.675 1.68-10.2 4.895-20.11 6.125-30.4.37-3 .295-6.275-1.425-8.875-1.8-2.42-5.03-2.71-7.75-3.35.99-3.53 2.29-7.315 1.15-10.975-1.57-4.65-6.565-7.415-11.275-7.675-4.94.22-9.065 4.11-10.275 8.8-4.19 13.75-8.33 27.495-12.6 41.225-.34-.25-1.035-.75-1.375-1-1.65-14.68-3.485-29.33-5.275-44-.47-3.29-.445-6.705-1.525-9.875-2.32-4.27-7.975-6.215-12.475-4.475-2.71 1.37-4.2 4.165-5.85 6.575-7.76-2.88-17.145 4.345-16.375 12.575.88 22.07 1.43 44.145 2 66.225-.08 1.61.135 3.94-1.675 4.7-1.72 1.06-3.53-.44-5.05-1.2-8.66-5.63-17.44-11.37-27.2-14.95-5.68-2.18-12.99-1.12-17 3.7-1.47 1.96-2.965 5.1-.925 7.2 7.6 8.09 18.16 13.315 24.05 23.025 4.4 6.28 7.615 13.52 13.425 18.7 3.85 4.1 8.05 8.2 10.3 13.45 1.9 5.47 1.175 11.37 1.225 17.05-21.41-7.21-40.095-22.44-50.725-42.45-10.11-18.55-13.43-40.775-8.85-61.425C23.13 73.415 31.535 58.2 43.025 46.05c15.74-15.94 37.435-26.155 59.925-26.975 1.2175-.06438 2.45607-.11193 3.675-.125z' color='#000' fill='#ccc' overflow='visible'/><path d='M106.975 0C92.535.39 78.08 3.04 64.8 8.85 41.04 18.95 21.235 37.98 10.175 61.3 4.515 72.91.8374 85.56245 0 98.425c-.40387 6.20353-.45048 12.44968 0 18.65C1.851 142.5516 13.86 167 32.25 184.75c18.1 17.72 42.61 28.51 67.85 30.25h14.025c21.4-1.55 42.355-9.38 59.225-22.7 19.74-15.36 33.91-37.82 38.95-62.35 4.57-21.33 2.42-44.055-6.15-64.125-9.41-22.45-26.71-41.46-48.2-52.9C142.53 4.395 124.93.48 107.4 0zm-.4 7.2c13.73578-.105 27.49 2.635 40.15 7.975 22.07 9.15 40.54 26.565 51.2 47.925 10.72 21.04 13.645 45.875 8.075 68.825-8 35.19-36.39 64.66-71.2 74.1-.68-8.65-1.83-17.405-.75-26.075.36-4.6 3.86-7.87 5.9-11.75 2.97-5.64 6.575-11.195 7.475-17.625 1.62-10.53 2.96-21.09 4.65-31.6 2.62-15.06 6.515-29.88 9.325-44.9 1.3-5.59-8.715-7.075-9.775-1.725-4.06 11.58-8.505 23.01-12.675 34.55 1.68 1.35 3.26 2.88 4.05 4.95-3.95-2.49-8.235-4.31-12.575-6 2.17-.27 4.345-.21 6.525-.05 5.29-14.17 10.68-28.29 15.45-42.65.86-2.72 2.025-5.74.825-8.55-1.78-3.81-7.075-5.025-10.575-2.875-1.62 1.15-2.25 3.165-2.95 4.925-4.81 13.73-8.865 27.72-13.525 41.5-.65 2.45-3.61 4.495-6 2.975-2.26-1.11-2.275-3.925-2.625-6.075-1.68-15.33-3.3-30.68-5.05-46-.37-2.97-.695-6.47-3.625-8.1-2.2-.76-5.205-.87-6.825 1.1-2.24 2.37-2.505 5.86-2.125 8.95 1.76 16.4 2.97 32.87 4.5 49.3 1.72.57 3.415 1.185 5.025 2.025-4.62.29-9.165 1.075-13.725 1.875 1.77-1.71 4.02-2.74 6.3-3.6-1.09-15.54-2.61-31.04-4-46.55-.37-2.06-.045-4.81-2.125-6.05-3.65-1.85-8.555.13-9.975 3.95-1.07 2.88-.53 6-.5 9 .66 21.7.485 43.4.675 65.1.11 3.97-4.575 5.91-7.925 4.9-4.84-1.38-8.745-4.775-13.125-7.125-6.25-3.46-12.295-7.57-19.175-9.7-4.61-1.47-9.47.915-11.95 4.875 6.37 6.02 13.545 11.18 19.725 17.4 5.43 6.03 9.29 13.26 14.45 19.5 5.03 5.34 10.34 10.6 14.05 17 2.7 4.56 2.48 10.05 2.95 15.15.28 6.45.805 12.9.425 19.35-22.89-5-43.8-18.3-58.1-36.85-14.27-18.2-21.9-41.44-21.3-64.55.63-23.35 9.49-46.48 25-64C46.795 23.85 69.16 11.645 92.9 8.275c4.5325-.675 9.0964-1.04 13.675-1.075zM93.95 130.975c9.28 2.65 18.715 9.13 20.725 19.15 1.25 6.87.315 14.575-3.675 20.425.69-5.51 2.39-11.075 1-16.625-1.73-9.96-9.61-17.675-18.45-21.875z' fill='#000' />");

            var totalSize = 348;
            var size = 100;
            var radius = 100;
            var svg = new[] { "Rock", "Paper", "Scissors", "Lizard", "Spock" }.Select((obj, i) => "<g transform='translate({0}, {1}) scale({2})'>{3}</g>".Fmt(
                /* 0 = x */ totalSize / 2 - size / 2 + radius * Math.Cos(Math.PI * 2 / 5 * i - Math.PI / 2),
                /* 1 = y */ totalSize / 2 - size / 2 + radius * Math.Sin(Math.PI * 2 / 5 * i - Math.PI / 2) + 10,
                /* 2 */ size / 215.0,
                /* 3 */ pristineSvgs[obj]
            )).JoinString(Environment.NewLine);
            var path = @"D:\c\KTANE\RockPaperScissorsLizardSpock\Manual\img\Rock-Paper-Scissors-Lizard-Spock.svg";
            File.WriteAllText(path, Regex.Replace(File.ReadAllText(path), @"(?<=<!--##-->).*(?=<!--###-->)", svg, RegexOptions.Singleline));
        }

        public static IEnumerable<Pt[]> GetMesh(XElement svg, bool half, double smoothness) => getMeshImpl(svg, half, smoothness).SelectMany(x => x);

        private static IEnumerable<IEnumerable<Pt[]>> getMeshImpl(XElement svg, bool half, double smoothness)
        {
            foreach (var path in svg.Elements().Where(p => p.Name.LocalName == "path").Where(p => (p.Attribute("class")?.Value == "half") == half))
            {
                var y = half ? 5 : 10;
                foreach (var points in DecodeSvgPath.Do(path.Attribute("d").Value, smoothness))
                {
                    var pointsArr = points.ToArray();

                    if (new PolygonD(pointsArr).Area() > 0)
                        Debugger.Break();

                    // Walls
                    yield return pointsArr.SelectConsecutivePairs(true, (p1, p2) => p1 == p2 ? null : new[] { pt(p1.X, 0, p1.Y), pt(p2.X, 0, p2.Y), pt(p2.X, y, p2.Y), pt(p1.X, y, p1.Y) }).Where(arr => arr != null);
                    // Face
                    yield return Triangulate(pointsArr).Select(ps => ps.Select(p => pt(p.X, y, p.Y)).ToArray());
                }
            }
        }
    }
}
