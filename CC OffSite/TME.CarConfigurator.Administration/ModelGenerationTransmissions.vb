Imports System.Collections.Generic
Imports TME.CarConfigurator.Administration.Assets
Imports TME.CarConfigurator.Administration.Enums

<Serializable()> Public NotInheritable Class ModelGenerationTransmissions
    Inherits BaseObjects.StronglySortedListBase(Of ModelGenerationTransmissions, ModelGenerationTransmission)

#Region " Business Properties & Methods "
    Friend Property Generation() As ModelGeneration
        Get
            If Me.Parent Is Nothing Then Return Nothing
            Return DirectCast(Me.Parent, ModelGeneration)
        End Get
        Private Set(ByVal value As ModelGeneration)
            Me.SetParent(value)
            If value.Mode = LocalizationMode.LocalConfiguration Then
                Me.AllowNew = Me.AllowEdit
                Me.AllowRemove = Me.AllowEdit
            End If
        End Set
    End Property

    Public Shadows Function Add(ByVal transmission As TransmissionInfo) As ModelGenerationTransmission
        If Me.Contains(transmission) Then Throw New Exceptions.ObjectAlreadyExists(Entity.TRANSMISSION, transmission)

        Dim _transmission = ModelGenerationTransmission.NewModelGenerationTransmission(transmission)
        MyBase.Add(_transmission)
        Return _transmission
    End Function

#End Region

#Region " Shared Factory Methods "

    Friend Shared Function NewModelGenerationTransmissions(ByVal generation As ModelGeneration) As ModelGenerationTransmissions
        Dim _transmissions As ModelGenerationTransmissions = New ModelGenerationTransmissions()
        _transmissions.Generation = generation
        Return _transmissions
    End Function
    Friend Shared Function GetModelGenerationTransmissions(ByVal generation As ModelGeneration, ByVal dataReader As SafeDataReader) As ModelGenerationTransmissions
        Dim _transmissions As ModelGenerationTransmissions = New ModelGenerationTransmissions()
        _transmissions.Generation = generation
        _transmissions.Fetch(dataReader)
        Return _transmissions
    End Function
    Friend Shared Function GetModelGenerationTransmissions(ByVal generation As ModelGeneration) As ModelGenerationTransmissions
        Dim _transmissions As ModelGenerationTransmissions = DataPortal.Fetch(Of ModelGenerationTransmissions)(New CustomCriteria(generation))
        _transmissions.Generation = generation
        Return _transmissions
    End Function

#End Region

#Region " Constructors "
    Private Sub New()
        'Prevent direct creation
        MarkAsChild()
        Me.AllowNew = False
        Me.AllowRemove = False
        Me.AllowEdit = Not MyContext.GetContext().IsSlaveRegionCountry
    End Sub
#End Region

#Region " Criteria "
    <Serializable()> Private Class CustomCriteria
        Inherits CommandCriteria

        Private ReadOnly _generationID As Guid

        Public Sub New(ByVal generation As ModelGeneration)
            _generationID = generation.ID
        End Sub
        Public Overloads Overrides Sub AddCommandFields(ByVal command As System.Data.SqlClient.SqlCommand)
            command.Parameters.AddWithValue("@GENERATIONID", _generationID)
        End Sub

    End Class
#End Region

#Region " Data Access "

    Friend Sub Synchronize()
        If Not AllowEdit Then Exit Sub

        AddMissingObjects()
        UpdateObjectStatuses()
    End Sub

    Private Sub AddMissingObjects()
        Dim toBeAdded = Generation.Cars.Where(Function(car) Not Contains(car.TransmissionID)).Select(Function(car) car.TransmissionID).Distinct().ToList()
        If Not toBeAdded.Any() Then Return

        Dim initialAllowNewValue = AllowNew
        Dim list = Transmissions.GetTransmissions()

        AllowNew = True
        For Each id In toBeAdded
            Add(list(id).GetInfo())
        Next
        AllowNew = initialAllowNewValue
    End Sub
    Private Sub UpdateObjectStatuses()
        For Each transmission In Me
            Dim availableCars = Generation.Cars.Where(Function(car) car.TransmissionID.Equals(transmission.ID)).ToList()
            transmission.Approved = availableCars.Any(Function(car) car.Approved)
            transmission.Preview = availableCars.Any(Function(car) car.Preview)
        Next
    End Sub

#End Region

End Class
<Serializable()> Public NotInheritable Class ModelGenerationTransmission
    Inherits BaseObjects.TranslateableBusinessBase
    Implements BaseObjects.ISortedIndex
    Implements BaseObjects.ISortedIndexSetter
    Implements IUpdatableAssetSet
    Implements IOwnedBy
    Implements IMasterObject

#Region " Business Properties & Methods "
    Private _objectID As Guid = Guid.Empty
    Private _code As String = String.Empty
    Private _name As String = String.Empty
    Private _numberOfGears As Integer
    Private _status As Integer
    Private _keyFeature As Boolean
    Private _brochure As Boolean
    Private _index As Integer
    Private _type As TransmissionTypeInfo
    Private _assetSet As AssetSet

    Private _masterID As Guid
    Private _masterDescription As String

    Public ReadOnly Property Generation() As ModelGeneration
        Get
            If Me.Parent Is Nothing Then Return Nothing
            Return DirectCast(Me.Parent, ModelGenerationTransmissions).Generation
        End Get
    End Property

    Private ReadOnly Property ObjectID() As Guid
        Get
            Return _objectID
        End Get
    End Property
    Public ReadOnly Property Code() As String
        Get
            Return _code
        End Get
    End Property
    Public ReadOnly Property Name() As String
        Get
            Return _name
        End Get
    End Property
    Public ReadOnly Property NumberOfGears() As Integer
        Get
            Return _numberOfGears
        End Get
    End Property
    Public ReadOnly Property Index() As Integer Implements BaseObjects.ISortedIndex.Index
        Get
            Return _index
        End Get
    End Property
    Friend WriteOnly Property SetIndex() As Integer Implements BaseObjects.ISortedIndexSetter.SetIndex
        Set(ByVal value As Integer)
            If Not _index.Equals(value) Then
                _index = value
                PropertyHasChanged("Index")
            End If
        End Set
    End Property
    Public Property KeyFeature() As Boolean
        Get
            Return _keyFeature
        End Get
        Set(ByVal value As Boolean)
            If Not value.Equals(Me.KeyFeature) Then
                _keyFeature = value
                PropertyHasChanged("KeyFeature")
            End If
        End Set
    End Property
    Public Property Brochure() As Boolean
        Get
            Return _brochure
        End Get
        Set(ByVal value As Boolean)
            If Not value.Equals(Me.Brochure) Then
                _brochure = value
                PropertyHasChanged("Brochure")
            End If
        End Set
    End Property

    Public Property Approved() As Boolean
        Get
            Return ((_status And Status.ApprovedForLive) = Status.ApprovedForLive)
        End Get
        Friend Set(ByVal value As Boolean)
            If Not value.Equals(Me.Approved) Then
                If Me.Approved Then
                    _status -= Status.ApprovedForLive
                    If Not Me.Declined Then _status += Status.Declined
                Else
                    _status += Status.ApprovedForLive
                    If Me.Declined Then _status -= Status.Declined
                End If
                PropertyHasChanged("Approved")
            End If
        End Set
    End Property
    Public Property Declined() As Boolean
        Get
            Return ((_status And Status.Declined) = Status.Declined)
        End Get
        Friend Set(ByVal value As Boolean)
            If Not value.Equals(Me.Declined) Then
                If Me.Declined Then
                    _status -= Status.Declined
                    If Not Me.Approved Then _status += Status.ApprovedForLive
                Else
                    _status += Status.Declined
                    If Me.Approved Then _status -= Status.ApprovedForLive
                End If
                PropertyHasChanged("Declined")
            End If
        End Set
    End Property
    Public Property Preview() As Boolean
        Get
            Return ((_status And Status.ApprovedForPreview) = Status.ApprovedForPreview)
        End Get
        Friend Set(ByVal value As Boolean)
            If Not value.Equals(Me.Preview) Then
                If Me.Preview Then
                    _status -= Status.ApprovedForPreview
                Else
                    _status += Status.ApprovedForPreview
                End If
                PropertyHasChanged("Preview")
            End If
        End Set
    End Property
    Public ReadOnly Property Visible() As Boolean
        Get
            Return Me.Approved OrElse Me.Preview
        End Get
    End Property
    Public ReadOnly Property Type() As TransmissionTypeInfo
        Get
            Return _type
        End Get
    End Property

    Public ReadOnly Property AssetSet() As AssetSet Implements IHAsAssetSet.AssetSet
        Get
            If _assetSet Is Nothing Then
                _assetSet = AssetSet.GetAssetSet(Me)
            End If
            Return _assetSet
        End Get
    End Property

    Public Sub ChangeReference(ByVal updatedAssetSet As AssetSet) Implements IUpdatableAssetSet.ChangeReference
        _assetSet = updatedAssetSet
    End Sub

    Public Property MasterID() As Guid Implements IMasterObject.MasterID
        Get
            Return _masterID
        End Get
        Set(ByVal value As Guid)
            If _masterID.Equals(value) Then Return
            _masterID = value
            If _masterID.Equals(Guid.Empty) Then _masterDescription = String.Empty
            PropertyHasChanged("Master")
        End Set
    End Property
    Public Property MasterDescription() As String Implements IMasterObject.MasterDescription
        Get
            Return _masterDescription
        End Get
        Set(ByVal value As String)
            If _masterDescription.Equals(value) Then Return

            _masterDescription = value
            PropertyHasChanged("Master")
        End Set
    End Property

    Public ReadOnly Property RefMasterID() As Guid Implements IMasterObjectReference.MasterID
        Get
            Return MasterID
        End Get
    End Property

    Public ReadOnly Property RefMasterDescription() As String Implements IMasterObjectReference.MasterDescription
        Get
            Return MasterDescription
        End Get
    End Property



    Public ReadOnly Property Owner() As String Implements IOwnedBy.Owner
        Get
            Return Environment.GlobalCountryCode
        End Get
    End Property

    Public Function GetInfo() As TransmissionInfo
        Return TransmissionInfo.GetTransmissionInfo(Me)
    End Function
#End Region

#Region " Business & Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ValidationRules.AddRule(DirectCast(AddressOf Administration.ValidationRules.MasterReference.Valid, Validation.RuleHandler), "Master")
    End Sub
#End Region

#Region " System.Object Overrides "

    Public Overloads Overrides Function ToString() As String
        Return Me.Name
    End Function
    Public Overloads Function Equals(ByVal obj As ModelGenerationTransmission) As Boolean
        Return Not (obj Is Nothing) AndAlso Me.Equals(obj.ID)
    End Function
    Public Overloads Function Equals(ByVal obj As TransmissionInfo) As Boolean
        Return Not (obj Is Nothing) AndAlso Me.Equals(obj.ID)
    End Function
    Public Overloads Function Equals(ByVal obj As Transmission) As Boolean
        Return Not (obj Is Nothing) AndAlso Me.Equals(obj.ID)
    End Function
    Public Overloads Overrides Function Equals(ByVal obj As Guid) As Boolean
        Return Me.ID.Equals(obj) OrElse Me.ObjectID.Equals(obj)
    End Function
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If TypeOf obj Is ModelGenerationTransmission Then
            Return Me.Equals(DirectCast(obj, ModelGenerationTransmission))
        ElseIf TypeOf obj Is TransmissionInfo Then
            Return Me.Equals(DirectCast(obj, TransmissionInfo))
        ElseIf TypeOf obj Is Transmission Then
            Return Me.Equals(DirectCast(obj, Transmission))
        ElseIf TypeOf obj Is Guid Then
            Return Me.Equals(DirectCast(obj, Guid))
        Else
            Return False
        End If
    End Function
#End Region

#Region " Framework Overrides "
    Public Overloads Overrides ReadOnly Property IsValid() As Boolean
        Get
            If Not MyBase.IsValid Then Return False
            If Not (_assetSet Is Nothing) AndAlso Not _assetSet.IsValid Then Return False
            Return True
        End Get
    End Property
    Public Overloads Overrides ReadOnly Property IsDirty() As Boolean
        Get
            If MyBase.IsDirty Then Return True
            If Not (_assetSet Is Nothing) AndAlso _assetSet.IsDirty Then Return True
            Return False
        End Get
    End Property

#End Region

#Region " Shared Factory Methods "
    Friend Shared Function NewModelGenerationTransmission(ByVal transmission As TransmissionInfo) As ModelGenerationTransmission
        Dim _transmission As ModelGenerationTransmission = New ModelGenerationTransmission()
        _transmission.Create(transmission)
        Return _transmission
    End Function
#End Region

#Region " Constructors "

    Private Sub New()
        'Prevent direct creation
        Me.MarkAsChild()
        Me.AlwaysUpdateSelf = True
        Me.AllowNew = False
        Me.AllowRemove = False
        With MyContext.GetContext()
            Me.AllowEdit = Not .IsRegionCountry OrElse .IsMainRegionCountry
            Me.AlwaysUpdateSelf = Me.AllowEdit
        End With
    End Sub

#End Region

#Region " Data Access "
    Private Overloads Sub Create(ByVal transmission As TransmissionInfo)
        MyBase.Create(transmission.ID)
        With transmission
            _objectID = Guid.NewGuid()
            _name = .Name
            _numberOfGears = .NumberOfGears
            _status = (Status.AvailableToNmscs + Status.Declined)
            _index = 0
            _type = transmission.Type
            _masterID = Guid.Empty
            _masterDescription = String.Empty
        End With
        Me.AllowNew = True
        Me.AllowRemove = True
        MarkNew()
    End Sub
    Protected Overrides Sub FetchSpecializedFields(ByVal dataReader As Common.Database.SafeDataReader)
        ID = dataReader.GetGuid("TRANSMISSIONID")
    End Sub
    Protected Overrides Sub FetchFields(ByVal dataReader As Common.Database.SafeDataReader)
        With dataReader
            _objectID = .GetGuid("ID")
            _code = .GetString("TRANSMISSIONCODE")
            _name = .GetString("TRANSMISSIONNAME")
            _numberOfGears = .GetInt16("TRANSMISSIONNUMBEROFGEARS")
            _status = .GetInt16("STATUSID")
            _keyFeature = .GetBoolean("KEYFEATURE")
            _brochure = .GetBoolean("BROCHURE")
            _index = .GetInt16("SORTORDER")
            _masterID = .GetGuid("MASTERID")
            _masterDescription = .GetString("MASTERDESCRIPTION")
            MyBase.AllowRemove = .GetString("OWNER").Equals(MyContext.GetContext().CountryCode, StringComparison.InvariantCultureIgnoreCase)
            _type = TransmissionTypeInfo.GetTransmissionTypeInfo(dataReader)
            _assetSet = AssetSet.GetAssetSet(Me, dataReader)
        End With
        MyBase.FetchFields(dataReader)
    End Sub
    Protected Overrides Sub AddInsertCommandSpecializedFields(ByVal command As System.Data.SqlClient.SqlCommand)
        command.CommandText = "updateModelGenerationTransmission"
        Me.AddCommandSpecializedFields(command)
    End Sub
    Protected Overrides Sub AddUpdateCommandSpecializedFields(ByVal command As System.Data.SqlClient.SqlCommand)
        Me.AddCommandSpecializedFields(command)
    End Sub
    Protected Overrides Sub AddDeleteCommandSpecializedFields(ByVal command As System.Data.SqlClient.SqlCommand)
        Me.AddCommandSpecializedFields(command)
    End Sub
    Private Sub AddCommandSpecializedFields(ByVal command As System.Data.SqlClient.SqlCommand)
        command.Parameters.AddWithValue("@ID", Me.ObjectID)
    End Sub

    Protected Overrides Sub AddInsertCommandFields(ByVal command As System.Data.SqlClient.SqlCommand)
        AddCommandFields(command)
    End Sub
    Protected Overrides Sub AddUpdateCommandFields(ByVal command As System.Data.SqlClient.SqlCommand)
        AddCommandFields(command)
    End Sub
    Private Sub AddCommandFields(ByVal command As System.Data.SqlClient.SqlCommand)
        command.Parameters.AddWithValue("@GENERATIONID", Me.Generation.ID)
        command.Parameters.AddWithValue("@TRANSMISSIONID", Me.ID)
        command.Parameters.AddWithValue("@STATUSID", _status)
        command.Parameters.AddWithValue("@KEYFEATURE", Me.KeyFeature)
        command.Parameters.AddWithValue("@BROCHURE", Me.Brochure)
        command.Parameters.AddWithValue("@SORTORDER", Me.Index)

        If Me.MasterID.Equals(Guid.Empty) Then
            command.Parameters.AddWithValue("@MASTERID", System.DBNull.Value)
            command.Parameters.AddWithValue("@MASTERDESCRIPTION", System.DBNull.Value)
        Else
            command.Parameters.AddWithValue("@MASTERID", Me.MasterID)
            command.Parameters.AddWithValue("@MASTERDESCRIPTION", Me.MasterDescription)
        End If
    End Sub

    Protected Overrides Sub UpdateChildren(ByVal transaction As System.Data.SqlClient.SqlTransaction)
        MyBase.UpdateChildren(transaction)
        If Not _assetSet Is Nothing Then _assetSet.Update(transaction)
    End Sub

#End Region

#Region "Base Object Overrides"

    Public Overrides ReadOnly Property ModelID() As Guid
        Get
            Return Generation.Model.ID
        End Get
    End Property

    Public Overrides ReadOnly Property GenerationID() As Guid
        Get
            Return Generation.ID
        End Get
    End Property

    Protected Friend Overrides Function GetBaseName() As String
        Return Me.Name
    End Function
    Public Overrides ReadOnly Property Entity As Entity
        Get
            Return Entity.MODELGENERATIONTRANSMISSION
        End Get
    End Property
#End Region
End Class