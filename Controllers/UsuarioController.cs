using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//----------------------------------
using apiRESTCheckUsuario.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace apiRESTCheckUsuario.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("check/usuario/spinusuario")]
        public clsApiStatus spInUsuario([FromBody] clsUsuario modelo)
        {
            // definicion de los objetos y modelos
            clsApiStatus objrespuesta = new clsApiStatus();
            JObject jsonresp = new JObject();
            //------------------------------------
            DataSet ds = new DataSet(); try
            {
                clsUsuario objusuario = new clsUsuario(modelo.cve, modelo.nombre, modelo.apellidoPaterno, modelo.apellidoMaterno, modelo.usuario, modelo.contrasena, modelo.ruta, modelo.tipo);
                ds = objusuario.spInUsuario();
                // Configuracion del objeto de salida
                objrespuesta.statusExec = true;
                objrespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                if (objrespuesta.ban == 0)
                {
                    objrespuesta.msg = "usuario registrado exitosamente";
                    jsonresp.Add("msData", "usuario registrado exitosamente");
                }
                else
                {
                    objrespuesta.msg = "usuario no registrado, verificar";
                    jsonresp.Add("msData", "usuario no registrado verificar");

                }
                objrespuesta.datos = jsonresp;
            }
            catch (Exception ex)
            {
                objrespuesta.statusExec = true;
                objrespuesta.ban = 1;
                objrespuesta.msg = "Fallo la insertacion del usuario, verificar ...";
                jsonresp.Add("msData", ex.InnerException.ToString());
                objrespuesta.datos = jsonresp;

            }


            return objrespuesta;

        }
        // endpoint para validación de acceso spValidarAcceso
        [HttpPost]
        [Route("check/usuario/spvalidaracceso")]
        public clsApiStatus spValidarAcceso([FromBody] clsUsuario modelo)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                // Creación del objeto del modelo clsUsuario
                clsUsuario objusuario = new clsUsuario(modelo.usuario, modelo.contrasena);
                ds = objusuario.spValidarAcceso();
                //configuracion del objeto de salida




                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario validado exitosamente";
                    jsonResp.Add("usu_nombre_completo", ds.Tables[0].Rows[0][1].ToString());
                    jsonResp.Add("usu_ruta", ds.Tables[0].Rows[0][2].ToString());
                    jsonResp.Add("usu_usuario", ds.Tables[0].Rows[0][3].ToString());
                    jsonResp.Add("tip_descripcion", ds.Tables[0].Rows[0][4].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "Usuario validado exitosamente";
                    jsonResp.Add("msgData", "Acceso denegado, verificar");
                }


            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;


            }

            return objRespuesta;

        }


        [HttpPost]
        [Route("check/usuario/spDelUsuario")]
        public clsApiStatus deleteUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                // Usa directamente el modelo que ya tiene la cve del usuario
                ds = modelo.spDelUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario No eliminado";
                    jsonResp.Add("usu_cve", ds.Tables[0].Rows[0][1].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "Usuario Eliminado exitosamente";
                    jsonResp.Add("msgData", "Usuario no Eliminado, verificar");
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        // endpoint para consulta de usuario
        [HttpGet]
        [Route("check/usuario/vwRptUsuario")]
        public clsApiStatus vwRptUsuario(string filtro)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuario(filtro);
                //Configuracion del objSalida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";
                //Formatear los datos recibidos (Data set) para 
                //enviarlos de salida(JSON)
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                //Configuracion del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }
        [HttpGet]
        [Route("check/usuario/vwRptUsuariofiltro")]
        public clsApiStatus vwRptUsuariofiltro(string filtro)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuariofiltro(filtro);
                //Configuracion del objSalida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";
                //Formatear los datos recibidos (Data set) para 
                //enviarlos de salida(JSON)
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                //Configuracion del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }

        [HttpGet]
        [Route("check/usuario/vwTipoUsuario")]
        public clsApiStatus vwTipoUsuario()
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwTipoUsuario();
                //Configuracion del objSalida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";
                //Formatear los datos recibidos (Data set) para 
                //enviarlos de salida(JSON)
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                //Configuracion del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }

        [HttpGet]
        [Route("check/usuario/vwRptUsuariocve")]
        public clsApiStatus vwRptUsuariocveo(string filtro)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuariocve(filtro);
                //Configuracion del objSalida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";
                //Formatear los datos recibidos (Data set) para 
                //enviarlos de salida(JSON)
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                //Configuracion del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }

        [HttpPost]
        [Route("check/usuario/spUpdUsuario")]
        public clsApiStatus UpdUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                ds = modelo.spUpdUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario actualizado exitosamente";
                    jsonResp.Add("usu_cve", ds.Tables[0].Rows[0][1].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "No se actualizó el usuario, verificar datos";
                    jsonResp.Add("msgData", "Usuario no modificado");
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

    }
}