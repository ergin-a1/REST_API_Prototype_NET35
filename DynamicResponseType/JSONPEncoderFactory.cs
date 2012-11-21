using System;
using System.Text;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;


namespace DamianBlog
{
    public class JSONPMessageProperty : IMessageProperty
    {
        public const string Name = "Microsoft.ServiceModel.Samples.JSONPMessageProperty";

        public IMessageProperty CreateCopy()
        {
            return new JSONPMessageProperty(this);
        }

        public JSONPMessageProperty()
        {
        }

        internal JSONPMessageProperty(JSONPMessageProperty other)
        {
            this.MethodName = other.MethodName;
            this.HeaderType = other.HeaderType;
        }

        public string MethodName { get; set; }

        public string HeaderType { get; set; }

    }

    public class JSONPEncoderFactory : MessageEncoderFactory
    {
        JSONPEncoder encoder;
        public JSONPEncoderFactory()
        {
            encoder = new JSONPEncoder();
        }

        public override MessageEncoder Encoder
        {
            get
            {
                return encoder;
            }
        }

        public override MessageVersion MessageVersion
        {
            get { return encoder.MessageVersion; }
        }

        //JSONP encoder
        class JSONPEncoder : MessageEncoder
        {
            private MessageEncoder encoder;

            public JSONPEncoder()
            {
                WebMessageEncodingBindingElement element = new WebMessageEncodingBindingElement();
                encoder = element.CreateMessageEncoderFactory().Encoder;
            }

            public override string ContentType
            {
                get
                {
                    return encoder.ContentType;
                }
            }

            public override string MediaType
            {
                get
                {
                    return encoder.MediaType;
                }
            }

            public override MessageVersion MessageVersion
            {
                get
                {
                    return encoder.MessageVersion;
                }
            }

            public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
            {
                return encoder.ReadMessage(buffer, bufferManager, contentType);
            }

            public override Message ReadMessage(System.IO.Stream stream, int maxSizeOfHeaders, string contentType)
            {

                return encoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
            }

            public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
            {
                ArraySegment<byte> byteArray = new ArraySegment<byte>();
                MemoryStream stream = new MemoryStream();
                StreamWriter sw = new StreamWriter(stream);
                XmlWriter writer = null;
                byte[] messageBytes = null;
                int messageLength = 0;
                int totalLength  = 0;
                byte[] totalBytes = null;

                string methodName = null;
                string headerType = null;


                if (message.Properties.ContainsKey(JSONPMessageProperty.Name))
                {
                    methodName = ((JSONPMessageProperty)(message.Properties[JSONPMessageProperty.Name])).MethodName;
                    headerType = ((JSONPMessageProperty)(message.Properties[JSONPMessageProperty.Name])).HeaderType;
                }

                if (headerType == null)
                {
                    writer = XmlWriter.Create(sw);
                    message.WriteMessage(writer);
                    writer.Close();

                    messageBytes = stream.GetBuffer();
                    messageLength = (int)stream.Position;
                    stream.Close();

                    totalLength = messageLength + messageOffset;
                    totalBytes = bufferManager.TakeBuffer(totalLength);
                    Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                    byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
                }
                else
                {
                    if (methodName == null)
                    {
                        if (headerType.Contains("text/xml") || headerType.Contains("application/xml"))
                        {
                            writer = XmlWriter.Create(sw);
                            message.WriteMessage(writer);
                            writer.Close();

                            messageBytes = stream.GetBuffer();
                            messageLength = (int)stream.Position;
                            stream.Close();

                            totalLength = messageLength + messageOffset;
                            totalBytes = bufferManager.TakeBuffer(totalLength);
                            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                            byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);

                        }
                        else if (headerType.Contains("application/json"))
                        {
                            writer = JsonReaderWriterFactory.CreateJsonWriter(stream);
                            message.WriteMessage(writer);
                            writer.Flush();

                            messageBytes = stream.GetBuffer();
                            messageLength = (int)stream.Position;
                            stream.Close();
                            writer.Close();

                            totalLength = messageLength + messageOffset;
                            totalBytes = bufferManager.TakeBuffer(totalLength);
                            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                            byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
                        }
                    }
                    else //if there is a JSONP request - check header
                    {

                        sw.Write(methodName + "( ");
                        sw.Flush();

                        writer = JsonReaderWriterFactory.CreateJsonWriter(stream);
                        message.WriteMessage(writer);
                        writer.Flush();
                        sw.Write(" );");
                        sw.Flush();

                        messageBytes = stream.GetBuffer();
                        messageLength = (int)stream.Position;
                        totalLength = messageLength + messageOffset;
                        totalBytes = bufferManager.TakeBuffer(totalLength);
                        Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                        byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
                        writer.Close();
                    }
                }

                return byteArray;
            }

            public override void WriteMessage(Message message, System.IO.Stream stream)
            {
                string methodName = null;
                if (message.Properties.ContainsKey(JSONPMessageProperty.Name))
                    methodName = ((JSONPMessageProperty)(message.Properties[JSONPMessageProperty.Name])).MethodName;

                if (methodName == null)
                {
                    encoder.WriteMessage(message, stream);
                    return;
                }

                WriteToStream(stream, methodName + "( ");
                encoder.WriteMessage(message, stream);
                WriteToStream(stream, " );");
            }

            public void WriteToStream(Stream stream, string content)
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(content);
                }
            }
            public override bool IsContentTypeSupported(string contentType)
            {
                return encoder.IsContentTypeSupported(contentType);
            }

        }
    }

}
