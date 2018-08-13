import keras
from keras.models import Sequential
from keras.layers import Dense, Dropout, Activation
from keras.optimizers import SGD

# Generate dummy data
import numpy as np
x_train = np.array([[0, 0],
                    [0, 1],
                    [1, 0],
                    [1, 1]])
y_train = np.array([[0, 0, 1, 1]]).T

model = Sequential()
# Dense(64) is a fully-connected layer with 64 hidden units.
# in the first layer, you must specify the expected input data shape:
# here, 20-dimensional vectors.
model.add(Dense(2, activation='relu', input_dim=2))
model.add(Dense(1, activation='relu'))

# sgd = SGD(lr=0.01, decay=1e-6, momentum=0.9, nesterov=True)
model.compile(loss='mean_squared_error',
              optimizer='sgd',
              metrics=['accuracy'])

model.fit(x_train, y_train,
          epochs=2000)
score = model.evaluate(x_train, y_train)
for layer in model.layers:
    weights = layer.get_weights() # list of numpy arrays
    print(weights)
