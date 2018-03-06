import React, { Component } from 'react';

class BadRoute extends Component {

  render() {
    return (
      <p className="text-center text-danger">Route does not exist</p>
    );
  }
}

export default BadRoute;
