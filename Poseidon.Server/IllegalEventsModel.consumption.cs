namespace Poseidon.Server;

public partial class IllegalEventsModel
{
    public class ModelInput
    {
        [ColumnName(@"mmsi")] public float Mmsi { get; set; }

        [ColumnName(@"latitude")] public float Latitude { get; set; }

        [ColumnName(@"longitude")] public float Longitude { get; set; }

        [ColumnName(@"start_timestamp")] public string Start_timestamp { get; set; }

        [ColumnName(@"end_timestamp")] public string End_timestamp { get; set; }

        [ColumnName(@"median_speed_knots")] public float Median_speed_knots { get; set; }

        [ColumnName(@"total_event_duration_hours")] public float Total_event_duration_hours { get; set; }

        [ColumnName(@"illegal")] public bool Illegal { get; set; }
    }

    public class ModelOutput
    {
        [ColumnName(@"mmsi")] public float Mmsi { get; set; }

        [ColumnName(@"latitude")] public float Latitude { get; set; }

        [ColumnName(@"longitude")] public float Longitude { get; set; }

        [ColumnName(@"start_timestamp")] public string Start_timestamp { get; set; }

        [ColumnName(@"end_timestamp")] public string End_timestamp { get; set; }

        [ColumnName(@"median_speed_knots")] public float Median_speed_knots { get; set; }

        [ColumnName(@"total_event_duration_hours")] public float Total_event_duration_hours { get; set; }

        [ColumnName(@"illegal")] public bool Illegal { get; set; }

        [ColumnName(@"Features")] public float[] Features { get; set; }

        [ColumnName(@"PredictedLabel")] public bool PredictedLabel { get; set; }

        [ColumnName(@"Score")] public float Score { get; set; }

        [ColumnName(@"Probability")] public float Probability { get; set; }
    }

    private static readonly string MlNetModelPath = Path.GetFullPath("IllegalEventsModel.zip");

    private static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new(CreatePredictEngine, true);

    public static ModelOutput Predict(ModelInput input)
    {
        PredictionEngine<ModelInput, ModelOutput> predEngine = PredictEngine.Value;
        return predEngine.Predict(input);
    }

    private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
    {
        MLContext mlContext = new();
        ITransformer mlModel = mlContext.Model.Load(MlNetModelPath, out DataViewSchema _);
        return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
    }
}