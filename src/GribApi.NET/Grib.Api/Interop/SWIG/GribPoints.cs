/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace Grib.Api.Interop.SWIG {

public class GribPoints : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal GribPoints(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(GribPoints obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~GribPoints() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          GribApiProxyPINVOKE.delete_GribPoints(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public GribContext context {
    set {
      GribApiProxyPINVOKE.GribPoints_context_set(swigCPtr, value.Reference);
    } 
	get {
		System.IntPtr pVal = GribApiProxyPINVOKE.GribPoints_context_get(swigCPtr);

		return pVal == System.IntPtr.Zero ? null : new GribContext(pVal);
	} 
  }

  public double[] latitudes {
    set {
      GribApiProxyPINVOKE.GribPoints_latitudes_set(swigCPtr, value);
    } 
	get
	{
		return GribApiProxyPINVOKE.GribPoints_latitudes_get(swigCPtr);
	} 
  }

  public double[] longitudes {
    set {
      GribApiProxyPINVOKE.GribPoints_longitudes_set(swigCPtr, value);
    } 
	get
	{
		return GribApiProxyPINVOKE.GribPoints_longitudes_get(swigCPtr);
	} 
  }

  public SizeT indexes {
    set {
      GribApiProxyPINVOKE.GribPoints_indexes_set(swigCPtr, ref  value.Value);
    } 
	get {
		System.IntPtr pVal = GribApiProxyPINVOKE.GribPoints_indexes_get(swigCPtr);
		
		// dereference the pointer
		System.UIntPtr val = (System.UIntPtr)System.Runtime.InteropServices.Marshal.PtrToStructure(pVal, typeof(System.UIntPtr));
		
		return (SizeT)val;
	} 
  }

  public SizeT groupStart {
    set {
      GribApiProxyPINVOKE.GribPoints_groupStart_set(swigCPtr, ref  value.Value);
    } 
	get {
		System.IntPtr pVal = GribApiProxyPINVOKE.GribPoints_groupStart_get(swigCPtr);
		
		// dereference the pointer
		System.UIntPtr val = (System.UIntPtr)System.Runtime.InteropServices.Marshal.PtrToStructure(pVal, typeof(System.UIntPtr));
		
		return (SizeT)val;
	} 
  }

  public SizeT groupLen {
    set {
      GribApiProxyPINVOKE.GribPoints_groupLen_set(swigCPtr, ref  value.Value);
    } 
	get {
		System.IntPtr pVal = GribApiProxyPINVOKE.GribPoints_groupLen_get(swigCPtr);
		
		// dereference the pointer
		System.UIntPtr val = (System.UIntPtr)System.Runtime.InteropServices.Marshal.PtrToStructure(pVal, typeof(System.UIntPtr));
		
		return (SizeT)val;
	} 
  }

  public SizeT nGroups {
    set {
      GribApiProxyPINVOKE.GribPoints_nGroups_set(swigCPtr, value.Value);
    } 
	get {
		System.UIntPtr val = GribApiProxyPINVOKE.GribPoints_nGroups_get(swigCPtr);
		
		return (SizeT)val;
	} 
  }

  public SizeT n {
    set {
      GribApiProxyPINVOKE.GribPoints_n_set(swigCPtr, value.Value);
    } 
	get {
		System.UIntPtr val = GribApiProxyPINVOKE.GribPoints_n_get(swigCPtr);
		
		return (SizeT)val;
	} 
  }

  public SizeT size {
    set {
      GribApiProxyPINVOKE.GribPoints_size_set(swigCPtr, value.Value);
    } 
	get {
		System.UIntPtr val = GribApiProxyPINVOKE.GribPoints_size_get(swigCPtr);
		
		return (SizeT)val;
	} 
  }

  public GribPoints() : this(GribApiProxyPINVOKE.new_GribPoints(), true) {
  }

}

}
