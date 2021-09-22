using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Gayme1
{
    public partial class Form2 : Form
    {
        Boolean[] isOpened = new Boolean[12];
        Boolean[] isOpenedOnce = new Boolean[12];
        int openedCardsCount = 0;
        int completelyOpenedCardsCount = 0;
        int tempFirstCardIndex = 0;
        
        List<Tuple<int, Image>> list = new List<Tuple<int, Image>>();


        PictureBox[] PicBox = new PictureBox[12];
        Image[] SavedPicBox = new Image[12];
        int[] SavedIndexesOfPictures = new int[12];

        public Form2()
        {
            InitializeComponent();
            Random random = new Random();

            
            list.Add(Tuple.Create<int, Image>(1,Properties.Resources.кот_1));//0
            list.Add(Tuple.Create<int, Image>(2,Properties.Resources.кот_2));//1
            list.Add(Tuple.Create<int, Image>(3,Properties.Resources.кот_3));//2
            list.Add(Tuple.Create<int, Image>(4,Properties.Resources.кот_4));//3
            list.Add(Tuple.Create<int, Image>(5,Properties.Resources.кот_5));//4
            list.Add(Tuple.Create<int, Image>(6,Properties.Resources.кот_6));//5
            list.Add(Tuple.Create<int, Image>(1,Properties.Resources.кот_1));//6
            list.Add(Tuple.Create<int, Image>(2,Properties.Resources.кот_2));//7
            list.Add(Tuple.Create<int, Image>(3,Properties.Resources.кот_3));//8
            list.Add(Tuple.Create<int, Image>(4,Properties.Resources.кот_4));//9
            list.Add(Tuple.Create<int, Image>(5,Properties.Resources.кот_5));//10
            list.Add(Tuple.Create<int, Image>(6,Properties.Resources.кот_6));//11



           
            //list.Add(Properties.Resources.обложка);//6
            //list.Add(Properties.Resources.обложкаПриНаведении);//7
            int x = 50, y = 50;// координаты первой карты

       

            for (int i = 0; i < PicBox.Length; i++)
            {
                PicBox[i] = new System.Windows.Forms.PictureBox();
                PicBox[i].BackgroundImage = Properties.Resources.обложка;
                PicBox[i].BackgroundImageLayout = ImageLayout.Stretch; 
                PicBox[i].Location = new System.Drawing.Point(x, y);
                PicBox[i].Size = new System.Drawing.Size(152, 215);
                PicBox[i].SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox[i].BackColor = Color.Transparent;

               
                this.Controls.Add(PicBox[i]);
                if(i == 3)
                {
                    x = 100;
                    y += 255;
                }else if (i == 7)
                {
                    x = 50;
                    y += 255;
                }else
                    x += 202;
                PicBox[i].MouseHover += new EventHandler(cardHover);
                PicBox[i].MouseLeave += new EventHandler(cardLeave);
                PicBox[i].Click += new EventHandler(cardClick);
                isOpened[i] = false;


            }
            



        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
        private void cardHover(object sender, EventArgs e)
        {
            int index = Array.IndexOf(PicBox, sender);
            if (!isOpened[index])
            {
                (sender as PictureBox).BackgroundImage = Properties.Resources.обложкаПриНаведении;
                GC.Collect();
            }
        }
        private void cardLeave(object sender, EventArgs e)
        {
            int index = Array.IndexOf(PicBox, sender);
            if (!isOpened[index])
            {
                (sender as PictureBox).BackgroundImage = Properties.Resources.обложка;
                GC.Collect();
            }
        }
        private void cardClick(object sender, EventArgs e)
        {
            Random random = new Random();
            int index = Array.IndexOf(PicBox, sender);

            if (isOpened[index] || openedCardsCount == 2)
                return;

            if (!isOpenedOnce[index])
            {
                int randomNumber = random.Next(list.Count);
                (sender as PictureBox).BackgroundImage = list[randomNumber].Item2;

                SavedPicBox[index] = list[randomNumber].Item2;

                
                SavedIndexesOfPictures[index] = list[randomNumber].Item1;
                
                list.RemoveAt(randomNumber);
                isOpened[index] = true;
                openedCardsCount++;
                isOpenedOnce[index] = true;
            }
            else
            {
                (sender as PictureBox).BackgroundImage = SavedPicBox[index];
                isOpened[index] = true;
                openedCardsCount++;
            }

            if (openedCardsCount == 1)
            {
                tempFirstCardIndex = index;

            }else if(openedCardsCount == 2)
            {
                if (SavedIndexesOfPictures[index] != SavedIndexesOfPictures[tempFirstCardIndex])
                {
                    Thread ClosingFirst = new Thread(new ParameterizedThreadStart(CardClosed));
                    ClosingFirst.Start(index);
                    Thread ClosingSecond = new Thread(new ParameterizedThreadStart(CardClosed));
                    ClosingSecond.Start(tempFirstCardIndex);
                }
                else 
                { 
                    openedCardsCount = 0;
                completelyOpenedCardsCount += 2;
                }

            }

            if (completelyOpenedCardsCount == 12)
            {
                Form3 newfrm1 = new Form3();
                newfrm1.Show();
                Close();
            }






            GC.Collect();
        }

        private void CardClosed(object x)
        {
           
            Thread.Sleep(1500);
            int index = (int)x;
            PicBox[index].BackgroundImage = Properties.Resources.обложка;
            isOpened[index] = false;
            openedCardsCount--;

            GC.Collect();
            

        }



    }
}
