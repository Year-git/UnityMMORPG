//WARNING: DON'T EDIT THIS FILE!!!
using Common;
using System;
using System.Reflection;

namespace Network
{
    public class MessageDispatch<T> : Singleton<MessageDispatch<T>>
    {
        public void Dispatch(T sender, SkillBridge.Message.NetMessageResponse message)
        {
            if (message.userRegister != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.userRegister); }
            if (message.userLogin != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.userLogin); }
            if (message.createChar != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.createChar); }
            if (message.gameEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameEnter); }
            if (message.gameLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameLeave); }
            if (message.mapCharacterEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapCharacterEnter); }
            if (message.mapCharacterLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapCharacterLeave); }
            if (message.mapEntitySync != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapEntitySync); }   
        }

        public void Dispatch(T sender, SkillBridge.Message.NetMessageRequest message)
        {
            if (message.userRegister != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.userRegister); }
            if (message.userLogin != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.userLogin); }
            if (message.createChar != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.createChar); }
            if (message.gameEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameEnter); }
            if (message.gameLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameLeave); }
            if (message.mapCharacterEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapCharacterEnter); }
            if (message.mapEntitySync != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapEntitySync); }
            if (message.mapTeleport != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapTeleport); }

            //Type type = typeof(SkillBridge.Message.NetMessageRequest);
            ////PropertyInfo[] properties = type.GetProperties();
            //FieldInfo[] fields = type.GetFields();
            //foreach (FieldInfo field in fields)
            //{
            //    var obj = field.GetValue(message);
            //    if (obj != null)
            //    {
            //        bool b = object.ReferenceEquals(obj, message.userLogin);
            //        if (b)
            //        {
            //            MessageDistributer<T>.Instance.RaiseEvent(sender, obj);
            //        }
            //    }
            //}
            ////foreach (PropertyInfo prop in properties)
            ////{
            ////    var obj = prop.GetValue(message, null);
            ////    if (obj != null)
            ////    {
            ////        bool b = object.ReferenceEquals(obj, message.userLogin);
            ////        if (b)
            ////        {
            ////            MessageDistributer<T>.Instance.RaiseEvent(sender, obj);
            ////        }
            ////    }
            ////}
        }
    }
}