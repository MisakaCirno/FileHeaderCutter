using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 砍头工具
{
    static class Program
    {
        public class IndexedItem<T>
        {
            public int Index;
            public T Item;
        }

        /// <summary>
        /// 返回带索引的迭代器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<IndexedItem<T>> Indexed<T>(this IEnumerable<T> source)
        {
            var num = 0;
            foreach (var i in source)
            {
                yield return new IndexedItem<T>() { Index = num, Item = i };
                num++;
            }

        }

        static void Main(string[] args)
        {
            const string SOURCE_PATH = @"C:\DB\art";
            const string NEW_PATH = @"C:\DB\artcut";

            var AllFiles=Directory.GetFiles(Path.GetFullPath(SOURCE_PATH), "*.*", SearchOption.AllDirectories);
            //读取目录中所有文件指定文件到AllFiles中

            foreach (var item in Indexed(AllFiles)) //从AllFiles中挨个取出文件信息到file中
            {
                var Percent = Convert.ToDouble(item.Index + 1) / AllFiles.Length;

                if (item.Index%100==0)
                {
                    Console.WriteLine($"当前：{item.Index + 1} 共：{AllFiles.Length} 进度：{Percent:P3}");
                }
                

                try
                {
                    var data = File.ReadAllBytes(item.Item);
                    var path = item.Item.Replace(SOURCE_PATH, @".\");
                    path = Path.GetFullPath(Path.Combine(NEW_PATH, path));
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.WriteAllBytes(path, data.Skip(8).ToArray());
                    //Console.WriteLine($"成功写出文件：{path}");
                }
                catch (Exception)
                {

                    Console.WriteLine($"处理文件失败：{item.Item}");
                }
            }
        }
    }
}
