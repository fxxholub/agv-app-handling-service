//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by ros2cs.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;

#nullable enable

namespace Handling_Service.Core.Ros2.Interfaces.Nav2
{
    /// <summary>
    /// Message interface definition for <c>nav2_msgs/msg/VoxelGrid</c>.
    /// </summary>
    [global::Rosidl.Runtime.TypeSupportAttribute("nav2_msgs/msg/VoxelGrid")]
    public unsafe partial class VoxelGrid : global::Rosidl.Runtime.IMessage
    {
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static string TypeSupportName => "nav2_msgs/msg/VoxelGrid";
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static global::Rosidl.Runtime.TypeSupportHandle GetTypeSupportHandle()
        {
            return new global::Rosidl.Runtime.TypeSupportHandle(_PInvoke(), global::Rosidl.Runtime.HandleType.Message);
            
            [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
            [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_typesupport_c", EntryPoint = "rosidl_typesupport_c__get_message_type_support_handle__nav2_msgs__msg__VoxelGrid")]
            static extern nint _PInvoke();
        }
        
        /// <summary>
        /// Create a new instance of <see cref="VoxelGrid"/> with fields initialized to specified values.
        /// </summary>
        /// <param name='header'>
        /// Originally defined as: <c><![CDATA[std_msgs/msg/Header header]]></c>
        /// </param>
        /// <param name='data'>
        /// Originally defined as: <c><![CDATA[uint32[] data]]></c>
        /// </param>
        /// <param name='origin'>
        /// Originally defined as: <c><![CDATA[geometry_msgs/msg/Point32 origin]]></c>
        /// </param>
        /// <param name='resolutions'>
        /// Originally defined as: <c><![CDATA[geometry_msgs/msg/Vector3 resolutions]]></c>
        /// </param>
        /// <param name='sizeX'>
        /// Originally defined as: <c><![CDATA[uint32 size_x]]></c>
        /// </param>
        /// <param name='sizeY'>
        /// Originally defined as: <c><![CDATA[uint32 size_y]]></c>
        /// </param>
        /// <param name='sizeZ'>
        /// Originally defined as: <c><![CDATA[uint32 size_z]]></c>
        /// </param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public VoxelGrid(
            global::Handling_Service.Core.Ros2.Interfaces.Std.Header? @header = null,
            uint[]? @data = null,
            global::Handling_Service.Core.Ros2.Interfaces.Geometry.Point32? @origin = null,
            global::Handling_Service.Core.Ros2.Interfaces.Geometry.Vector3? @resolutions = null,
            uint @sizeX = 0,
            uint @sizeY = 0,
            uint @sizeZ = 0
        )
        {
            Header = @header ?? new global::Handling_Service.Core.Ros2.Interfaces.Std.Header();
            Data = @data ?? global::System.Array.Empty<uint>();
            Origin = @origin ?? new global::Handling_Service.Core.Ros2.Interfaces.Geometry.Point32();
            Resolutions = @resolutions ?? new global::Handling_Service.Core.Ros2.Interfaces.Geometry.Vector3();
            SizeX = @sizeX;
            SizeY = @sizeY;
            SizeZ = @sizeZ;
        }
        
        
        /// <summary>
        /// Create a new instance of <see cref="VoxelGrid"/>, and copy its data from the specified <see cref="Priv"/> structure.
        /// </summary>
        /// <param name="priv">The <see cref="Priv"/> structure to be copied from.</param>
        /// <param name="textEncoding">Text encoding of the strings in the <see cref="Priv"/> structure and its containing structures, if any.</param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public VoxelGrid(in Priv priv, global::System.Text.Encoding textEncoding)
        {
            this.Header = new global::Handling_Service.Core.Ros2.Interfaces.Std.Header(in priv.Header, textEncoding);
            this.Data = priv.Data.AsSpan().ToArray();
            this.Origin = new global::Handling_Service.Core.Ros2.Interfaces.Geometry.Point32(in priv.Origin, textEncoding);
            this.Resolutions = new global::Handling_Service.Core.Ros2.Interfaces.Geometry.Vector3(in priv.Resolutions, textEncoding);
            this.SizeX = priv.SizeX;
            this.SizeY = priv.SizeY;
            this.SizeZ = priv.SizeZ;
        }
        
        
        /// <summary>
        /// Originally defined as: <c><![CDATA[std_msgs/msg/Header header]]></c>
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public global::Handling_Service.Core.Ros2.Interfaces.Std.Header Header { get; set; }
        
        /// <summary>
        /// Originally defined as: <c><![CDATA[uint32[] data]]></c>
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public uint[] Data { get; set; }
        
        /// <summary>
        /// Originally defined as: <c><![CDATA[geometry_msgs/msg/Point32 origin]]></c>
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public global::Handling_Service.Core.Ros2.Interfaces.Geometry.Point32 Origin { get; set; }
        
        /// <summary>
        /// Originally defined as: <c><![CDATA[geometry_msgs/msg/Vector3 resolutions]]></c>
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public global::Handling_Service.Core.Ros2.Interfaces.Geometry.Vector3 Resolutions { get; set; }
        
        /// <summary>
        /// Originally defined as: <c><![CDATA[uint32 size_x]]></c>
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public uint SizeX { get; set; }
        
        /// <summary>
        /// Originally defined as: <c><![CDATA[uint32 size_y]]></c>
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public uint SizeY { get; set; }
        
        /// <summary>
        /// Originally defined as: <c><![CDATA[uint32 size_z]]></c>
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public uint SizeZ { get; set; }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public void WriteTo(nint data, global::System.Text.Encoding textEncoding)
        {
            WriteTo(ref global::System.Runtime.CompilerServices.Unsafe.AsRef<Priv>(data.ToPointer()), textEncoding);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public void WriteTo(ref Priv priv, global::System.Text.Encoding textEncoding)
        {
            this.Header.WriteTo(ref priv.Header, textEncoding);
            priv.Data.CopyFrom(this.Data);
            this.Origin.WriteTo(ref priv.Origin, textEncoding);
            this.Resolutions.WriteTo(ref priv.Resolutions, textEncoding);
            priv.SizeX = this.SizeX;
            priv.SizeY = this.SizeY;
            priv.SizeZ = this.SizeZ;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static global::Rosidl.Runtime.IMessage CreateFrom(nint data, global::System.Text.Encoding textEncoding)
        {
            return new VoxelGrid(in global::System.Runtime.CompilerServices.Unsafe.AsRef<Priv>(data.ToPointer()), textEncoding);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static nint UnsafeCreate()
        {
            return new(Priv.Create());
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static void UnsafeDestroy(nint data)
        {
            Priv.Destroy((Priv*)data);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static bool UnsafeInitialize(nint data)
        {
            return Priv.TryInitialize(out System.Runtime.CompilerServices.Unsafe.AsRef<Priv>(data.ToPointer()));
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static void UnsafeFinalize(nint data)
        {
            Priv.Finalize(ref System.Runtime.CompilerServices.Unsafe.AsRef<Priv>(data.ToPointer()));
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static bool UnsafeInitializeSequence(int size, nint data)
        {
            return PrivSequence.TryInitialize(size, out System.Runtime.CompilerServices.Unsafe.AsRef<PrivSequence>(data.ToPointer()));
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
        public static void UnsafeFinalizeSequence(nint data)
        {
            PrivSequence.Finalize(ref System.Runtime.CompilerServices.Unsafe.AsRef<PrivSequence>(data.ToPointer()));
        }
        
        /// <summary>
        /// Blittable native structure for <c>nav2_msgs/msg/VoxelGrid</c>.
        /// </summary>
        [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
        public partial struct Priv : global::System.IEquatable<Priv>, global::System.IDisposable
        {
            /// <summary>
            /// Originally defined as: <c><![CDATA[std_msgs/msg/Header header]]></c>
            /// </summary>
            public global::Handling_Service.Core.Ros2.Interfaces.Std.Header.Priv Header;
            
            /// <summary>
            /// Originally defined as: <c><![CDATA[uint32[] data]]></c>
            /// </summary>
            public global::Rosidl.Runtime.Interop.UInt32Sequence Data;
            
            /// <summary>
            /// Originally defined as: <c><![CDATA[geometry_msgs/msg/Point32 origin]]></c>
            /// </summary>
            public global::Handling_Service.Core.Ros2.Interfaces.Geometry.Point32.Priv Origin;
            
            /// <summary>
            /// Originally defined as: <c><![CDATA[geometry_msgs/msg/Vector3 resolutions]]></c>
            /// </summary>
            public global::Handling_Service.Core.Ros2.Interfaces.Geometry.Vector3.Priv Resolutions;
            
            /// <summary>
            /// Originally defined as: <c><![CDATA[uint32 size_x]]></c>
            /// </summary>
            public uint SizeX;
            
            /// <summary>
            /// Originally defined as: <c><![CDATA[uint32 size_y]]></c>
            /// </summary>
            public uint SizeY;
            
            /// <summary>
            /// Originally defined as: <c><![CDATA[uint32 size_z]]></c>
            /// </summary>
            public uint SizeZ;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public Priv()
            {
                ThrowIfNonSuccess(TryInitialize(out this));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public Priv(Priv src)
                : this(in src)
            {
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public Priv(in Priv src)
                : this()
            {
                CopyFrom(in src); 
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public Priv(Priv* src)
                : this()
            {
                CopyFrom(src); 
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void Dispose()
            {
                Finalize(ref this);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void CopyFrom(Priv src)
            {
                ThrowIfNonSuccess(TryCopy(in src, out this));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void CopyFrom(in Priv src)
            {
                ThrowIfNonSuccess(TryCopy(in src, out this));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void CopyFrom(Priv* src)
            {
                fixed (Priv* pThis = &this)
                {
                    ThrowIfNonSuccess(TryCopy(src, pThis));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            private static bool TryCopy(in Priv input, out Priv output)
            {
                fixed (Priv* pInput = &input, pOutput = &output)
                {
                    return TryCopy(pInput, pOutput);
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public bool Equals(Priv other)
            {
                return Priv.AreEqual(in other, in this);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public override bool Equals(object? obj) => obj is Priv s ? this.Equals(s) : false;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public override int GetHashCode()
            {
                var __hashCode = new global::System.HashCode();
                __hashCode.Add(this.Header);
                __hashCode.Add(this.Data);
                __hashCode.Add(this.Origin);
                __hashCode.Add(this.Resolutions);
                __hashCode.Add(this.SizeX);
                __hashCode.Add(this.SizeY);
                __hashCode.Add(this.SizeZ);
                return __hashCode.ToHashCode();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static bool operator ==(Priv lhs, Priv rhs)
            {
                return lhs.Equals(rhs);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static bool operator !=(Priv lhs, Priv rhs)
            {
                return !(lhs == rhs);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static Priv* Create()
            {
                return _PInvoke();
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__create")]
                static extern Priv* _PInvoke();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static void Destroy(Priv* msg)
            {
                _PInvoke(msg);
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__destroy")]
                static extern void _PInvoke(Priv* msg);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static bool TryInitialize(out Priv msg)
            {
                fixed (Priv* pMsg = &msg)
                {
                    return _PInvoke(pMsg);
                }
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__init")]
                static extern bool _PInvoke(Priv* msg);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static void Finalize(ref Priv msg)
            {
                fixed (Priv* pMsg = &msg)
                {
                    _PInvoke(pMsg);
                }
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__fini")]
                static extern void _PInvoke(Priv* msg);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            private static bool AreEqual(in Priv lhs, in Priv rhs)
            {
                fixed (Priv* plhs = &lhs, prhs = &rhs)
                {
                    return _PInvoke(plhs, prhs);
                }
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__are_qual")]
                static extern bool _PInvoke(Priv* lhs, Priv* rhs);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            private static bool TryCopy(Priv* input, Priv* output)
            {
                return _PInvoke(input, output);
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__copy")]
                static extern bool _PInvoke(Priv* input, Priv* output);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static void ThrowIfNonSuccess(bool ret, [global::System.Runtime.CompilerServices.CallerMemberNameAttribute]
            string caller = "")
            {
                if (!ret)
                {
                    throw new global::Rosidl.Runtime.RosidlException($"An error occurred when calling 'global::Handling_Service.Core.Ros2.Interfaces.Nav2.VoxelGrid.Priv.{caller}'.");
                }
            }
        }
        
        /// <summary>
        /// Blittable native sequence structure for <c>nav2_msgs/msg/VoxelGrid</c>.
        /// </summary>
        [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
        public partial struct PrivSequence : global::System.IEquatable<PrivSequence>, global::System.IDisposable
        {
            private Priv* __data;
            
            private nuint __size;
            
            private nuint __capacity;
            
            public int Size => (int)__size;
            
            public int Capcacity => (int)__capacity;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public PrivSequence()
                : this(0)
            {
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public PrivSequence(int size)
            {
                ThrowIfNonSuccess(TryInitialize(size, out this));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public PrivSequence(PrivSequence src)
                : this(in src)
            {
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public PrivSequence(in PrivSequence src)
                : this()
            {
                CopyFrom(in src); 
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public PrivSequence(PrivSequence* src)
                : this()
            {
                CopyFrom(src); 
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public PrivSequence(System.ReadOnlySpan<Priv> src)
                : this(src.Length)
            {
                src.CopyTo(AsSpan());
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void Dispose()
            {
                Finalize(ref this);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public System.Span<Priv> AsSpan()
            {
                return new(__data, Size);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void CopyFrom(System.ReadOnlySpan<Priv> src)
            {
                Finalize(ref this);
                ThrowIfNonSuccess(TryInitialize(src.Length, out this));
                src.CopyTo(AsSpan());
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void CopyFrom(PrivSequence src)
            {
                ThrowIfNonSuccess(TryCopy(in src, out this));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void CopyFrom(in PrivSequence src)
            {
                ThrowIfNonSuccess(TryCopy(in src, out this));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public void CopyFrom(PrivSequence* src)
            {
                fixed (PrivSequence* pThis = &this)
                {
                    ThrowIfNonSuccess(TryCopy(src, pThis));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            private static bool TryCopy(in PrivSequence input, out PrivSequence output)
            {
                fixed (PrivSequence* pInput = &input, pOutput = &output)
                {
                    return TryCopy(pInput, pOutput);
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public bool Equals(PrivSequence other)
            {
                return PrivSequence.AreEqual(in other, in this);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public override bool Equals(object? obj) => obj is PrivSequence s ? this.Equals(s) : false;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public override int GetHashCode()
            {
                return global::System.HashCode.Combine((nint)__data, __size, __capacity);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static bool operator ==(PrivSequence lhs, PrivSequence rhs)
            {
                return lhs.Equals(rhs);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static bool operator !=(PrivSequence lhs, PrivSequence rhs)
            {
                return !(lhs == rhs);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static PrivSequence* Create()
            {
                return _PInvoke();
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__Sequence__create")]
                static extern PrivSequence* _PInvoke();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static void Destroy(PrivSequence* msg)
            {
                _PInvoke(msg);
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__Sequence__destroy")]
                static extern void _PInvoke(PrivSequence* msg);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static bool TryInitialize(int size, out PrivSequence msg)
            {
                fixed (PrivSequence* pMsg = &msg)
                {
                    return _PInvoke(pMsg, (uint)size);
                }
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__Sequence__init")]
                static extern bool _PInvoke(PrivSequence* msg, nuint size);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static void Finalize(ref PrivSequence msg)
            {
                fixed (PrivSequence* pMsg = &msg)
                {
                    _PInvoke(pMsg);
                }
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__Sequence__fini")]
                static extern void _PInvoke(PrivSequence* msg);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            private static bool AreEqual(in PrivSequence lhs, in PrivSequence rhs)
            {
                fixed (PrivSequence* plhs = &lhs, prhs = &rhs)
                {
                    return _PInvoke(plhs, prhs);
                }
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__Sequence__are_qual")]
                static extern bool _PInvoke(PrivSequence* lhs, PrivSequence* rhs);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            private static bool TryCopy(PrivSequence* input, PrivSequence* output)
            {
                return _PInvoke(input, output);
                
                [global::System.Runtime.InteropServices.SuppressGCTransitionAttribute]
                [global::System.Runtime.InteropServices.DllImportAttribute("nav2_msgs__rosidl_generator_c", EntryPoint = "nav2_msgs__msg__VoxelGrid__Sequence__copy")]
                static extern bool _PInvoke(PrivSequence* input, PrivSequence* output);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ros2cs", "1.3.2+da40d8cd913b1d7f1523f4fd2ce6a86ea7d47c2e")]
            public static void ThrowIfNonSuccess(bool ret, [global::System.Runtime.CompilerServices.CallerMemberNameAttribute]
            string caller = "")
            {
                if (!ret)
                {
                    throw new global::Rosidl.Runtime.RosidlException($"An error occurred when calling 'global::Handling_Service.Core.Ros2.Interfaces.Nav2.VoxelGrid.PrivSequence.{caller}'.");
                }
            }
        }
    }
}
