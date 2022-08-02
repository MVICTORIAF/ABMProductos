using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace ABMProductos
{
    public partial class frmProducto : Form
    {
        bool nuevo = true;
      
        AccesoDatos oBD = new AccesoDatos(); //creo un objeto de la clase acceso de datos y la creo que mi abm es una asociacion 
        List<Producto> lProductos = new List<Producto>(); //lista dinamica de la lista de Productos
      

        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            cargarCombo(cboMarca, "Marcas"); //metodo cargar combo , combo + nombre de la tabla como un string 
            cargarLista(lstProducto, "Productos"); //metodo para cargar la lista + nombre de la lista 
            habilitar(false);
            limpiar();
            
        }

        private void cargarLista(ListBox lista, string nombreTabla)
        {

            lProductos.Clear(); //limpio la lista 
            oBD.leerBD("SELECT * FROM " + nombreTabla); //invoca el data reader y haceme un select para consulta 
            while (oBD.pLector.Read()) //si ese lector esta cargado y puede leer 
            {
                Producto p = new Producto(); //creo un objeto producto 
                if (!oBD.pLector.IsDBNull(0)) //si no esta vacio y no esta nulo en la columna cero 
                    p.pCodigo = oBD.pLector.GetInt32(0); //
                if (!oBD.pLector.IsDBNull(1))
                    p.pDetalle = oBD.pLector.GetString(1);
                if (!oBD.pLector.IsDBNull(2))
                    p.pTipo = oBD.pLector.GetInt32(2);
                if (!oBD.pLector.IsDBNull(3))
                    p.pMarca = oBD.pLector.GetInt32(3);
                if (!oBD.pLector.IsDBNull(4))
                    p.pPrecio = oBD.pLector.GetDouble(4);
                if (!oBD.pLector.IsDBNull(5))
                    p.pFecha = oBD.pLector.GetDateTime(5);

                lProductos.Add(p); //en mi lista de productos agrega un p que es un producto
            }
            oBD.desconectar(); //desconectar 
            lista.Items.Clear(); //mostramos la lista de producto 
            for (int i = 0; i < lProductos.Count; i++)
            {
                lista.Items.Add(lProductos[i].ToString()); //cargue la lista y agregue lo que tengo lproducto + string 
            }
            lista.SelectedIndex = 0; //para que siempre arranque desde el primer lugar que es cero 
        }

        private void cargarCombo(ComboBox combo, string nombreTabla) //pasame el combo y el nombre de la tabla que queres que te cargue 
        {
            DataTable tabla = oBD.consultarBD("SELECT * FROM " + nombreTabla + " ORDER BY 2"); //
            combo.DataSource = tabla; //es mi tabla 
            combo.ValueMember = tabla.Columns[0].ColumnName; //"idMarca" campo identificador 
            combo.DisplayMember = tabla.Columns[1].ColumnName; //"nombreMarca" campo descriptor 
            combo.DropDownStyle = ComboBoxStyle.DropDownList; //para que el usuario no pueda editar el combo box 
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
                if (txtCodigo.Text == "") 
                {
                    MessageBox.Show("Debe ingresar un codigo");
                    return;
                }

                if (txtDetalle.Text == "")
                {
                    MessageBox.Show("Debe ingresar un Detalle");
                    return;
                }
                if (txtPrecio.Text == "")
                {
                    MessageBox.Show("Debe ingresar un Precio");
                    return;
                }

                //if (cboMarca.SelectedIndex < 0) 
                //{
                //    MessageBox.Show("Debe ingresar una marca");
                //    return;
                //}



                if (DateTime.Today.Year - dtpFecha.Value.Year > 10)
                {
                    MessageBox.Show("No puede registrar un producto mayor a 10 años...");
                    dtpFecha.Focus();
                    return;
                }

            Producto p = new Producto(); //instancio un nuevo producto 
            p.pCodigo = int.Parse(txtCodigo.Text);
            p.pDetalle = txtDetalle.Text;
            //p.pMarca = (int)cboMarca.SelectedValue; antes staba asi 
            if (cboMarca.SelectedValue != null)
            {
                p.pMarca = (int)cboMarca.SelectedValue;
            }
            else
            {
                p.pMarca = 0;
            }
        
            if (rbtNoteBook.Checked)
                p.pTipo = 1;
            else
                p.pTipo = 2;
            p.pPrecio = double.Parse(txtPrecio.Text);
            p.pFecha = dtpFecha.Value;


            if (nuevo)
            {
                string insertSQL = $"INSERT INTO Productos VALUES ({p.pCodigo},'{p.pDetalle}',{p.pTipo},{p.pMarca},{p.pPrecio},'{p.pFecha.ToString("yyyy/MM/dd")}')";

                oBD.actualizarBD(insertSQL);
                cargarLista(lstProducto, "Productos");

            }
            else
            {

             string updateSql2 = $"UPDATE Productos SET detalle='{p.pDetalle}',tipo={p.pTipo},marca={p.pMarca},precio={p.pPrecio},fecha='{p.pFecha.ToString("yyyy/MM/dd")}' Where codigo = {p.pCodigo}";

                oBD.actualizarBD(updateSql2);
                cargarLista(lstProducto, "Productos");
                //nuevo = true;
            }

            habilitar(false);
            limpiar();

            btnGrabar.Enabled = false;
            MessageBox.Show("Operacion Exitosa");

        }

        private void limpiar()
        {
            txtCodigo.Text = "";
            txtDetalle.Text = "";
            cboMarca.SelectedIndex = -1;
            rbtNoteBook.Checked = false;
            rbtNetBook.Checked = false;
            txtPrecio.Text = "";
            dtpFecha.Value = DateTime.Today;
            chb.Checked = false;
        }

        private void habilitar(bool v)
        {
            txtCodigo.Enabled = v;
            txtDetalle.Enabled = v;
            cboMarca.Enabled = v;
            rbtNoteBook.Enabled = v;
            rbtNetBook.Enabled = v;
            txtPrecio.Enabled = v;
            dtpFecha.Enabled = v;
            btnGrabar.Enabled = v;
            btnCancelar.Enabled = v;
            btnNuevo.Enabled = !v;
            btnEditar.Enabled = !v;
            btnBorrar.Enabled = !v;
            btnSalir.Enabled = !v;
            lstProducto.Enabled = !v;
            chb.Enabled = v;

            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro de abandonar la aplicación ?",
                  "SALIR", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                  MessageBoxDefaultButton.Button2) == DialogResult.Yes)

                this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
            habilitar(false);
            nuevo = false;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            txtCodigo.Focus();
            nuevo = false;
            txtCodigo.Enabled = false;
            cboMarca.Enabled = false;

            
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            nuevo = true;
            habilitar(true);
            cboMarca.Enabled = false;
            limpiar();
            txtCodigo.Focus();
            
        }


        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Está seguro de eliminar a " + lProductos[lstProducto.SelectedIndex] + " ?",
              "BORRANDO",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Warning,
              MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {

                string deleteSql = "DELETE FROM productos WHERE codigo=" + lProductos[lstProducto.SelectedIndex].pCodigo;
                oBD.actualizarBD(deleteSql);
                cargarLista(lstProducto, "Productos");

            }


        }

        private void lstProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cargarCampos(lstProducto.SelectedIndex);
        }

        private void cargarCampos(int posicion)
        {
            txtCodigo.Text = lProductos[posicion].pCodigo.ToString();
            txtDetalle.Text = lProductos[posicion].pDetalle;
            //chb.Checked = lProductos[posicion].p; // no lo tengo completo por que falta definirlo en la clase prodcuto 
            cboMarca.SelectedValue = lProductos[posicion].pMarca;
            if (lProductos[posicion].pTipo == 1)
                rbtNoteBook.Checked = true;
            else
                rbtNoteBook.Checked = true;
            txtPrecio.Text = lProductos[posicion].pPrecio.ToString();
            dtpFecha.Value = lProductos[posicion].pFecha;
        }

        private void chb_CheckedChanged(object sender, EventArgs e)
        {
            if (chb.Checked)
            {
                cboMarca.Enabled = true;
            }
            else
            {
                cboMarca.Enabled = false;
            }
        }
    } 

    }
