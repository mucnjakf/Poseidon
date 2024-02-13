namespace Poseidon.Server;

public partial class IllegalEventsModel
{
    public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
    {
        IEstimator<ITransformer> pipeline = BuildPipeline(context);
        ITransformer model = pipeline.Fit(trainData);

        return model;
    }

    private static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
    {
        EstimatorChain<CalibratorTransformer<NaiveCalibrator>> pipeline = mlContext.Transforms.ReplaceMissingValues(new[]
            {
                new InputOutputColumnPair(@"mmsi", @"mmsi"),
                new InputOutputColumnPair(@"latitude", @"latitude"),
                new InputOutputColumnPair(@"longitude", @"longitude"),
                new InputOutputColumnPair(@"median_speed_knots", @"median_speed_knots"),
                new InputOutputColumnPair(@"total_event_duration_hours", @"total_event_duration_hours")
            })
            .Append(mlContext.Transforms.Concatenate(@"Features", 
                @"mmsi", @"latitude", @"longitude", @"median_speed_knots", @"total_event_duration_hours"))
            .Append(mlContext.BinaryClassification.Trainers.FastTree(new FastTreeBinaryTrainer.Options
            {
                NumberOfLeaves = 9, MinimumExampleCountPerLeaf = 30, NumberOfTrees = 9, MaximumBinCountPerFeature = 242,
                FeatureFraction = 0.92287342655942, LearningRate = 0.44454931952006, LabelColumnName = @"illegal", FeatureColumnName = @"Features"
            }))
            .Append(mlContext.BinaryClassification.Calibrators.Naive(labelColumnName: @"illegal", scoreColumnName: @"Score"));

        return pipeline;
    }
}