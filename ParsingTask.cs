using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {        
        var answer = lines.
            Select(line => line.Split(";"))
            .Where(l => l.Length == 3)
            .Where(l =>
            {               
                return int.TryParse(l[0], out _) && Enum.TryParse<SlideType>(l[1].Trim(), true, out _);
            })
            .ToDictionary(line => int.Parse(line[0]), line => new SlideRecord
            (int.Parse(line[0]), Enum.Parse<SlideType>(line[1].Trim(), true), line[2]));
        return answer;
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Select(line => line.Split(";"))
                .Skip(1)
                .Select(words =>
                {
                    try
                    {
                        var userID = int.Parse(words[0]);
                        var slideID = int.Parse(words[1]);
                        var dateTime = DateTime.Parse(words[2] +" "+ words[3]);
                        var slideType = slides[slideID].SlideType;
                        return new VisitRecord(userID, slideID, dateTime, slideType);
                    }
                    catch
                    {
                        StringBuilder line = new StringBuilder();
                        line.AppendJoin(";", words);
                        throw new FormatException("Wrong line [" + line.ToString() + "]");
                    }
                });
    }
}