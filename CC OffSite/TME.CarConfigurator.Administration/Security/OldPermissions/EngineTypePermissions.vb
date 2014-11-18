Imports TME.CarConfigurator.Administration.Security.OldPermissions.Base

Namespace Security.OldPermissions
    <Serializable()> Public Class EngineTypePermissions
        Inherits Permissions

#Region " Constructors "
        Friend Sub New(ByVal context As MyContext)
            If context.IsGlobal() Then
                If Thread.CurrentPrincipal.IsInRole("ISG Administrator") OrElse Thread.CurrentPrincipal.IsInRole("Core Administrator") Then
                    Create = True
                    Update = True
                    Delete = True
                    Activate = True
                    Approve = True
                    Sort = True
                    ViewAssets = True
                End If
            End If
            ViewAssets = context.CanViewAssets
            UsesNonVATPrice = context.UsesNonVATPrice
        End Sub
#End Region

    End Class
End Namespace