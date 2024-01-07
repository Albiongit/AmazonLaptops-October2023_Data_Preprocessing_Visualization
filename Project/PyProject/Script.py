import pandas as pd
import pandas as pd
import plotly.express as px
from plotly.subplots import make_subplots

# Load the dataset
file_path = "amazon_laptops_processed.csv"
df = pd.read_csv(file_path)

fig_3d_date_age_sex = px.scatter_3d(df, x='Price', y='Ram', z='HardDisk', color='Price')

# Create subplot
fig = make_subplots(rows=1, cols=2, specs=[ [{'type': 'scatter3d'}, {'type': 'scatter3d'}]],
                    subplot_titles=['3D scatter plot with Price, Ram, and HardDisk'])


# Add 3D scatter plots to subplot
fig.add_trace(fig_3d_date_age_sex.data[0], row=1, col=1)

# Update layout
fig.update_layout(height=800, showlegend=True)

# Show the combined plot
fig.show()