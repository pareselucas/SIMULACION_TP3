﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP3V1._0
{
    public partial class Exponencial : Form
    {
        double[] num;
        public Exponencial()
        {
            InitializeComponent();
        }

        public void generarAleatoriosExponencial(int cantidad,float lambda)
        {
            Random r = new Random();
            num = new double[cantidad];
            double[] aleatorio = new double[cantidad];
            for(int i = 0; i < cantidad; i++)
            {
                double random = r.NextDouble();
                random = Math.Truncate(100 * random) / 100;
                double x = (-1/lambda) * (Math.Log(1 - random));
                x = Math.Truncate(100*x)/100;
                num[i] = x;
                aleatorio[i] = random;
            }

            for (int i = 0; i < aleatorio.Length; i++)
            {
                dgv_exponencial.Rows.Add();
                dgv_exponencial.Rows[i].Cells[0].Value = i+1;
                dgv_exponencial.Rows[i].Cells[1].Value = aleatorio[i];
                dgv_exponencial.Rows[i].Cells[2].Value = num[i];
            }

        }

        public DataGridView generarGridGrafico(GraficoExponencial g)
        {
            double max = 0;
            for(int i = 0; i < num.Length; i++)
            {
                if (max < num[i])
                {
                    max = num[i];
                }
            }
            double min = max;

            for(int i = 0; i < num.Length; i++)
            {
                if (num[i] < min)
                {
                    min = num[i];
                }
            }

            double rango = ((max-min) / double.Parse(txt_cantidadExp.Text));
            double limteInferior = min;
            double limteSuperior = limteInferior + rango;
            int contadorFrecuencia = 0;
            int contInterv = 0;
            double sum = 0;
            double frecEsperada;
            double probEsperada;
            while (limteSuperior <= max)
            {
                for (int i = 0; i < num.Length; i++)
                {
                    double numero = num[i];
                    if ((numero >= limteInferior && numero < limteSuperior) || (numero == max && limteSuperior == max))
                    {

                        contadorFrecuencia++;
                    }
                }

                g.dgv_graficoExponencial.Rows.Add();
                g.dgv_graficoExponencial.Rows[contInterv].Cells[0].Value = limteInferior;
                g.dgv_graficoExponencial.Rows[contInterv].Cells[1].Value = limteSuperior;
                g.dgv_graficoExponencial.Rows[contInterv].Cells[2].Value = contadorFrecuencia;
                probEsperada = (1 - Math.Pow(Math.E, ((-double.Parse(txt_lambda.Text)) * limteSuperior))) - (1 - Math.Pow(Math.E, ((-double.Parse(txt_lambda.Text)) * limteInferior)));
                g.dgv_graficoExponencial.Rows[contInterv].Cells[5].Value = probEsperada;
                frecEsperada = probEsperada * int.Parse(txt_tamañoE.Text);
                g.dgv_graficoExponencial.Rows[contInterv].Cells[3].Value = frecEsperada;

                g.dgv_graficoExponencial.Rows[contInterv].Cells[4].Value = Math.Truncate(100 * (Math.Pow((contadorFrecuencia - frecEsperada), 2) / frecEsperada)) / 100;
                contInterv++;
                sum += Math.Truncate(100 * (Math.Pow((contadorFrecuencia - frecEsperada), 2) / frecEsperada)) / 100; 
                contadorFrecuencia = 0;
                limteInferior = limteSuperior;
                limteSuperior = limteInferior + rango;
            }
            return g.dgv_graficoExponencial;
        }
       

        private void btn_generarE_Click(object sender, EventArgs e)
        {
            generarAleatoriosExponencial(int.Parse(txt_tamañoE.Text), float.Parse(txt_lambda.Text));
        }

        private void btn_graficarExp_Click(object sender, EventArgs e)
        {
            GraficoExponencial g = new GraficoExponencial();

            g.dgv_graficoExponencial = generarGridGrafico(g);

            for (int i = 0; i < g.dgv_graficoExponencial.RowCount - 1; i++)
            {
                string nombre = g.dgv_graficoExponencial.Rows[i].Cells[0].Value.ToString() + "-" + g.dgv_graficoExponencial.Rows[i].Cells[1].Value.ToString();
                g.ch_exponencial.Series["Series1"].Points.AddXY(nombre, double.Parse(g.dgv_graficoExponencial.Rows[i].Cells[2].Value.ToString()));
                g.ch_exponencial.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            }


            g.Show();
        }

        private void Exponencial_Load(object sender, EventArgs e)
        {

        }

        private void btn_salir_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btn_limpiar_Click(object sender, EventArgs e)
        {
            txt_cantidadExp.Text = "";
            txt_lambda.Text = "";
            txt_tamañoE.Text = "";
            dgv_exponencial.Rows.Clear();
        }
    }
}
